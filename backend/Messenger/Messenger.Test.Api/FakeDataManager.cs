using Messenger.Repository.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Test.Api
{
    [TestClass]
    public class FakeDataManager
    {
        private IServiceProvider _serviceProvider;

        public FakeDataManager(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        [TestMethod]
        public async System.Threading.Tasks.Task<Database.User> CreateFakeUser()
        {
            var _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            Database.User result = await _userRepository.CreateAsync(user);

            Assert.IsNotNull(result);

            return result;
        }

        //[TestMethod]
        //public async System.Threading.Tasks.Task<Brand> CreateFakeBrandByNameAsync(string name)
        //{
        //    var _brandRepository = _serviceProvider.GetRequiredService<IBrandRepository>();

        //    Brand brand = new Brand();
        //    brand.Name = name;
        //    Brand result = await _brandRepository.CreateAsync(brand);

        //    Assert.IsNotNull(result);

        //    return result;
        //}

        //[TestMethod]
        //public async System.Threading.Tasks.Task<Brand> CreateFakeBrandAsync()
        //{
        //    return await CreateFakeBrandByNameAsync("Test Brand");
        //}

        //[TestMethod]
        //public async Task<User> CreateFakeUserAsync(bool isAdmin = false)
        //{
        //    var _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();

        //    User user = new User();
        //    user.Email = "lenfant.chris.dev@outlook.com";
        //    user.Password = SecurityHelper.HashPassword("password");
        //    user.State = State.Active;
        //    user.RemainingAttempts = 10;
        //    user.IsAdmin = isAdmin;

        //    User result = await _userRepository.CreateAsync(user);

        //    Assert.IsNotNull(result);
        //    return result;
        //}

        //[TestMethod]
        //public Mall CreateFakeMall(User user)
        //{
        //    return CreateFakeMallByName(user, "Test Mall");
        //}

        //[TestMethod]
        //public Mall CreateFakeMallWithCoordinates(User user, decimal longitude, decimal latitude)
        //{
        //    var _mallRepository = _serviceProvider.GetRequiredService<IMallRepository>();

        //    Mall mall = new Mall();
        //    mall.UserId = user.Id;
        //    mall.Name = "Test Mall";
        //    mall.Longitude = longitude;
        //    mall.Latitude = latitude;

        //    Mall result = _mallRepository.Create(mall);

        //    Assert.IsNotNull(result);
        //    return result;
        //}

        //[TestMethod]
        //public List<Mall> CreateFakeMallBulk(int number, User user)
        //{
        //    List<Mall> malls = new List<Mall>();

        //    for (int i = 0; i < number; i++)
        //    {
        //        malls.Add(this.CreateFakeMall(user));
        //    }

        //    return malls;
        //}

        //[TestMethod]
        //public Mall CreateFakeMallByName(User user, string name)
        //{
        //    var _mallRepository = _serviceProvider.GetRequiredService<IMallRepository>();

        //    Mall mall = new Mall();
        //    mall.UserId = user.Id;
        //    mall.Name = "Test Mall";
        //    mall.Longitude = 50;
        //    mall.Latitude = 50;


        //    Mall result = _mallRepository.Create(mall);

        //    Assert.IsNotNull(result);
        //    return result;
        //}

        //[TestMethod]
        //public Store CreateFakeStoreByName(Mall mall, Brand brand, string name)
        //{
        //    var _storeRepository = _serviceProvider.GetRequiredService<IStoreRepository>();

        //    Store store = new Store();
        //    store.Name = name;
        //    store.MallId = mall.Id;
        //    store.BrandId = brand.Id;
        //    store.Longitude = 50;
        //    store.Latitude = 50;

        //    Store result = _storeRepository.Create(store);

        //    Assert.IsNotNull(result);
        //    return result;
        //}

        //[TestMethod]
        //public Store CreateFakeStore(Mall mall, Brand brand)
        //{
        //    return CreateFakeStoreByName(mall, brand, "Test Store");
        //}
    }
}
