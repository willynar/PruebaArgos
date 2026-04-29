using POCArgos.Logic;
using Test.Fixtures;
using Test.Tests.Mocks;

namespace Test.Tests.Integration
{
    public class CatalogsTests : IAsyncLifetime
    {
        private readonly IntegrationTestFixture _fixture = new();
        private Catalogs _catalogsService = null!;
        public async Task InitializeAsync()
        {
            await _fixture.InitializeAsync();
            _catalogsService = new Catalogs(_fixture.DbContext);
        }

        public async Task DisposeAsync()
        {
            await _fixture.DisposeAsync();
        }

        [Fact]
        public async Task RetrieveAllOrderStatusesFromDatabase()
        {
            var statuses = await _catalogsService.GetOrderStatusesAsync();

            Assert.NotNull(statuses);
            Assert.NotEmpty(statuses);
            Assert.Equal(4, statuses.Count);
        }


        [Fact]
        public async Task RetrieveAllShippingMethodsFromDatabase()
        {
            var methods = await _catalogsService.GetShippingMethodsAsync();

            Assert.NotNull(methods);
            Assert.NotEmpty(methods);
            Assert.Equal(3, methods.Count);
        }
    }
}
