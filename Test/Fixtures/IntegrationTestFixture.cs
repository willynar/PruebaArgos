using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using POCArgos.Models;

namespace Test.Fixtures
{
    public class IntegrationTestFixture : IAsyncLifetime
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext DbContext { get; private set; } = null!;

        public IntegrationTestFixture()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _configuration = configBuilder.Build();
        }

        public async Task InitializeAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            DbContext = new ApplicationDbContext(options);
            await DbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await DbContext.DisposeAsync();
        }
    }
}
