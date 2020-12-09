using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Messenger.Database;
using Messenger.EmailSending.Models;
using Messenger.Facade.Helpers;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class UserService : BaseService, IUserService
    {
        protected readonly AppSettings _appSettings;

        public UserService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings, IOptions<AppSettings> appSettings) : base(serviceProvider, jwtSettings)
        {
            this._appSettings = appSettings.Value;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> CreateUser(User user)
        {
            PrepareUser(user);

            string validationError = ValidateUser(user);

            if(validationError != null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, validationError, null);
            }

            //Hash password to store in database
            string hashedPassword = SecurityHelper.HashPassword(user.Password);
            user.Password = hashedPassword;
            user.State = UserState.WaitingActivation;

            //Create user
            User result = await _userRepository.CreateAsync(user);

            if (result == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "", null);
            }

            // Generate JWT token to authenticate user account activation request
            var tokenValue = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret(_jwtSettings.Key)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(72).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();


            //Create and save account activation token
            Token token = new Token()
            {
                Type = TokenType.AccountActivation,
                UserId = user.Id,
                Value = tokenValue,
                Date = DateTime.Now
            };

            Token tokenResult = await _tokenRepository.CreateAsync(token);

            if (tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);

            //Create email model
            ActivateAccountEmailModel emailModel = new ActivateAccountEmailModel()
            {
                link = _appSettings.WebAppUrl + "/activate_account/" + tokenResult.Value,
                userName = user.FirstName + " " + user.LastName,
                userEmail = user.Email
            };

            _emailSender.SendActivateAccountEmail(emailModel);

            return new ReturnApiObject(HttpStatusCode.Created, ResponseType.Success, "", result);
        }

        /// <summary>
        /// Get user by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnApiObject GetUser(int id)
        {
            UserSessionModel user = _userRepository.List().Where(x => x.Id == id)
                .Select(x=>new UserSessionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    State = (int)x.State
                })
                .SingleOrDefault();

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", user);
        }

        /// <summary>
        /// Update user informations
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> UpdateUserInformations(UserBasicModel updatedUser)
        {
            User user = _userRepository.List().Where(x => x.Id == updatedUser.Id)
             .SingleOrDefault();

            user.LastName = updatedUser.LastName;
            user.FirstName = updatedUser.FirstName;

            PrepareUser(user);

            User result = await _userRepository.UpdateAsync(user);

            if (result == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "", null);
            }

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", result);
        }

        /// <summary>
        /// Authenticate user on the system by saving a JWT Token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ReturnApiObject Login(string email, string password)
        {
            //Get the hashed password provided
            string hashedPassword = SecurityHelper.HashPassword(password);

            //Get user from database
            User user = _userRepository.List().Where(x => x.Email == email && x.Password == hashedPassword)
                .SingleOrDefault();

            if (user == null)
            {
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error, "BAD_CREDENTIALS", null);
            }
            else if (user.State == UserState.WaitingActivation)
            {
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error, "NOT_ACTIVATED", null);
            }
            else
            {
                //Generate a token to authenticate the user in the system
                var tokenString = GenerateJSONWebToken(user);

                UserBasicModel userModel = new UserBasicModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", new { token = tokenString, user = userModel });
            }
        }

        /// <summary>
        /// Check if the email is already used by another user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ReturnApiObject EmailAlreadyExists(string email)
        {
            bool result = IsUserEmailUsed(email);

            //If email is already used
            if (result)
            {
                return new ReturnApiObject(HttpStatusCode.Conflict, ResponseType.Error);
            }
            //If not
            else
            {
                return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
            }
        }

        /// <summary>
        /// Generate JWT Token to authenticate user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJSONWebToken(User user)
        {
            int role = -1;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                 new Claim(ClaimTypes.Name, user.Id.ToString()),
                 new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
             };

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
           _jwtSettings.Issuer,
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        /// <summary>
        /// Prepare user data before saving
        /// </summary>
        /// <param name="user"></param>
        private void PrepareUser(User user)
        {
            user.FirstName = user.FirstName.Trim();
            user.LastName = user.LastName.Trim();
            user.Email = user.Email.Trim();
        }

        /// <summary>
        /// Validate user data (email, password) before saving
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string ValidateUser(User user)
        {
            //Check email is unique
            if (IsUserEmailUsed(user.Email))
            {
                return "EMAIL_ALREADY_USED";
            }

            if(user.Password == null)
            {
                return "NO_PASSWORD";
            }

            //Check password strength
            if (!SecurityHelper.PasswordMatchRegex(user.Password))
            {
                return "PASSWORD_TOO_WEAK";
            }

            return null;
        }


        /// <summary>
        /// Check if an email is already used by another user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsUserEmailUsed(string email)
        {
            return _userRepository.List().Where(x => x.Email == email).Any();
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> UpdateUserPassword(int userId, string oldPassword, string newPassword)
        {
            User user = await _userRepository.GetById(userId);

            if(user == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            string hashedOldPwd = SecurityHelper.HashPassword(oldPassword);

            if(user.Password != hashedOldPwd)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "OLD_PASSWORD_DIFFERENT", null);
            }

            if (oldPassword == newPassword)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "SAME_NEW_PASSWORD", null);
            }

            if (!SecurityHelper.PasswordMatchRegex(newPassword))
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "NEW_PASSWORD_TOO_WEAK", null);
            }

            user.Password = SecurityHelper.HashPassword(newPassword);


            User userUpdated = await _userRepository.UpdateAsync(user);

            if (userUpdated == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);

        }

        /// <summary>
        /// Save token to change password when forgotten and send mail
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ForgotPassword(string email)
        {
            User user = _userRepository.List().Where(x => x.Email == email).SingleOrDefault();

            if(user == null)
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);

            // Generate JWT token to authenticate user reset password request
            var tokenValue = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret(_jwtSettings.Key)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();


            //Create and save reset password token
            Token token = new Token()
            {
                Type = TokenType.ForgotPassword,
                UserId = user.Id,
                Value = tokenValue,
                Date = DateTime.Now
            };

            Token tokenResult = await _tokenRepository.CreateAsync(token);

            if(tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);

            //Create email model
            ResetPasswordEmailModel emailModel = new ResetPasswordEmailModel()
            {
                link = _appSettings.WebAppUrl+"/reset_password/"+ tokenResult.Value,
                userName = user.FirstName + " " + user.LastName,
                userEmail = user.Email
            };

            //Send reset password email
            _emailSender.SendResetPasswordEmail(emailModel);

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Validate token to reset password
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ValidateTokenPasswordReset(string token)
        {
            Token tokenResult = _tokenRepository.List().Where(x=>x.Value == token && x.Type == TokenType.ForgotPassword).SingleOrDefault();

            if(tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error);

            //Token verification (expiration, key, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, _jwtSettings.Key, verify: true);
            }
            catch (TokenExpiredException)
            {
                await _tokenRepository.DeleteAsync(tokenResult);
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }
            catch (SignatureVerificationException)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Reset the user password by using email token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ResetPassword(string token, string newPassword)
        {
            Token tokenResult = _tokenRepository.List().Where(x => x.Value == token && x.Type == TokenType.ForgotPassword).SingleOrDefault();

            if (tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error);

            //Token verification (expiration, key, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, _jwtSettings.Key, verify: true);
            }
            // If token expired
            catch (TokenExpiredException)
            {
                await _tokenRepository.DeleteAsync(tokenResult);
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }
            // If token signature verification failed
            catch (SignatureVerificationException)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            // Get user from token
            User user = _userRepository.List().Where(x => x.Id == tokenResult.UserId).SingleOrDefault();

            if(user == null)
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);

            string newPasswordHash = SecurityHelper.HashPassword(newPassword);

            // If new password is too weak
            if (!SecurityHelper.PasswordMatchRegex(newPassword))
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "NEW_PASSWORD_TOO_WEAK", null);
            }

            // Set user new password
            user.Password = newPasswordHash;

            // Update user
            User userUpdated = await _userRepository.UpdateAsync(user);

            if (userUpdated == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            // Delete token
            await _tokenRepository.DeleteAsync(tokenResult);

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Get the user profile informations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnApiObject GetUserProfile(int id)
        {
            UserProfileModel userProfile = _userRepository.List().Where(x => x.Id == id).Select(x => new UserProfileModel()
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).SingleOrDefault();

            if(userProfile == null)
            {
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error);
            }

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", userProfile);
        }

        /// <summary>
        /// Get the profile picture of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] GetUserProfilePicture(int id)
        {
            User user = _userRepository.List().Where(x => x.Id == id).SingleOrDefault();

            return user.ProfilePicture;
        }

        /// <summary>
        /// Change the profile picture of the user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pictureBase64"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ChangeUserProfilePicture(int id, string pictureBase64)
        {
            User user = _userRepository.List().Where(x => x.Id == id).SingleOrDefault();

            user.ProfilePicture = Convert.FromBase64String(pictureBase64);

            User userUpdated = await _userRepository.UpdateAsync(user);

            if(userUpdated == null)
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Delete the profile picture of the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> DeleteUserProfilePicture(int id)
        {
            User user = _userRepository.List().Where(x => x.Id == id).SingleOrDefault();

            user.ProfilePicture = null;

            User userUpdated = await _userRepository.UpdateAsync(user);

            if(userUpdated == null)
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Check the token validity and activate the user account
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ActivateAccount(string token)
        {
            Token tokenResult = _tokenRepository.List().Where(x => x.Value == token && x.Type == TokenType.AccountActivation).SingleOrDefault();

            if (tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error);

            //Token verification (expiration, key, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, _jwtSettings.Key, verify: true);
            }
            // If token expired
            catch (TokenExpiredException)
            {
                await _tokenRepository.DeleteAsync(tokenResult);
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }
            // If token signature verification failed
            catch (SignatureVerificationException)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            // Get user from token
            User user = _userRepository.List().Where(x => x.Id == tokenResult.UserId).SingleOrDefault();

            if(user == null)
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);

            // Change user state to activate account
            user.State = UserState.Activated;

            User userUpdated = await _userRepository.UpdateAsync(user);

            if (userUpdated == null)
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success);
        }

        /// <summary>
        /// Send another email to activate an user account linked to the email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> ResendAccountActivationEmail(string email)
        {
            //Get user
            User user = _userRepository.List().Where(x=>x.Email == email).SingleOrDefault();

            if (user == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error, "", null);
            }

            // Generate JWT token to authenticate user account activation request
            var tokenValue = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret(_jwtSettings.Key)
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(72).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();


            //Create and save account activation token
            Token token = new Token()
            {
                Type = TokenType.AccountActivation,
                UserId = user.Id,
                Value = tokenValue,
                Date = DateTime.Now
            };

            Token tokenResult = await _tokenRepository.CreateAsync(token);

            if (tokenResult == null)
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);

            //Create email model
            ActivateAccountEmailModel emailModel = new ActivateAccountEmailModel()
            {
                link = _appSettings.WebAppUrl + "/activate_account/" + tokenResult.Value,
                userName = user.FirstName + " " + user.LastName,
                userEmail = user.Email
            };


            bool result =  await _emailSender.SendActivateAccountEmail(emailModel);

            return (result ? new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success) : new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error));
        }
    }

}
