using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Messenger.Facade.Helpers
{
    public class SecurityHelper
    {
        public static string regexPassword = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";
        public static string dictionnary = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Find if the password match the security requirement: more than 8 caracters, 1 number, 1 upper 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool PasswordMatchRegex(string password)
        {
            return Regex.IsMatch(password, regexPassword);
        }

        /// <summary>
        /// Hash the password to store in database
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {

            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }

        /// <summary>
        /// Generate password in dictionnary with a length take in parameter
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GeneratePassword(int number)
        {
            string password = "";
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {
                int randomInt = random.Next(0, dictionnary.Length);
                password += dictionnary[randomInt];
            }

            return password;
        }
    }
}
