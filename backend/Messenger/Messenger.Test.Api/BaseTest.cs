using Messenger.Api;
using Messenger.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Messenger.Test.Api
{
    [TestClass]
    public class BaseTest
    {
        public IServiceProvider _serviceProvider;
        public FakeDataManager FakeDataManager;
        SqliteConnection connection = new SqliteConnection("DataSource=:memory:");

        public BaseTest()
        {
            OpenConnection();

            var options = new DbContextOptionsBuilder<MessengerDbContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new MessengerDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            var services = new ServiceCollection();
            services.AddDbContext<MessengerDbContext>(options => options.UseSqlite(connection));

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();
            var startup = new Startup(configuration);

            startup.ConfigureServices(services);

            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddSingleton<DiagnosticListener>(diagnosticSource);
            services.AddLogging();
            services.AddMvc();

            this._serviceProvider = services.BuildServiceProvider();

            this.FakeDataManager = new FakeDataManager(_serviceProvider);
        }

        public void OpenConnection()
        {
            this.connection.Open();
        }

        public void CloseConnection()
        {
            this.connection.Close();
        }
    }
}