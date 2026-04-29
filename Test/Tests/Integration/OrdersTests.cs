using POCArgos.Logic;
using POCArgos.Models;
using Test.Fixtures;
using Test.Tests.Mocks;

namespace Test.Tests.Integration
{
    public class OrdersTests : IAsyncLifetime
    {
        private readonly IntegrationTestFixture _fixture = new();
        private Orders _ordersService = null!;
        private OrdersMock OrdersMock = new();
        public async Task InitializeAsync()
        {
            await _fixture.InitializeAsync();
            _ordersService = new Orders(_fixture.DbContext);
        }

        public async Task DisposeAsync()
        {
            await _fixture.DisposeAsync();
        }

        [Fact]
        public async Task CreateNewOrderInDatabase()
        {
            var newOrder = OrdersMock.CreateBasicOrder();

            var createdOrder = await _ordersService.CreateOrderAsync(newOrder);

            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.Id > 0);
            Assert.Equal("Test Customer", createdOrder.CustomerName);
            Assert.Equal(150.00m, createdOrder.TotalAmount);
        }

        [Fact]
        public async Task RetrieveCreatedOrderById()
        {
            var newOrder = OrdersMock.CreateJohnSmithOrder();

            var created = await _ordersService.CreateOrderAsync(newOrder);
            var retrieved = await _ordersService.GetOrderByIdAsync(created.Id);

            Assert.NotNull(retrieved);
            Assert.Equal(created.Id, retrieved.Id);
            Assert.Equal("John Smith", retrieved.CustomerName);
            Assert.Equal(250.50m, retrieved.TotalAmount);
        }

        [Fact]
        public async Task UpdateOrderSuccessfully()
        {
            var initialOrder = OrdersMock.CreateOrderForUpdate();

            var created = await _ordersService.CreateOrderAsync(initialOrder);

            created.CustomerName = "Updated Name";
            created.TotalAmount = 200.00m;
            created.OrderStatusId = 2;

            var updated = await _ordersService.UpdateOrderAsync(created.Id, created);

            Assert.True(updated);

            var retrieved = await _ordersService.GetOrderByIdAsync(created.Id);
            Assert.NotNull(retrieved);
            Assert.Equal("Updated Name", retrieved.CustomerName);
            Assert.Equal(200.00m, retrieved.TotalAmount);
            Assert.Equal(2, retrieved.OrderStatusId);
        }

        [Fact]
        public async Task RetrieveMultipleOrdersWithAllRelations()
        {
            var order1 = OrdersMock.CreateCustomerOneOrder();
            var order2 = OrdersMock.CreateCustomerTwoOrder();

            await _ordersService.CreateOrderAsync(order1);
            await _ordersService.CreateOrderAsync(order2);

            var allOrders = await _ordersService.GetOrdersAsync();

            var retrievedOrder1 = allOrders.FirstOrDefault(o => o.CustomerName == "Customer One");
            var retrievedOrder2 = allOrders.FirstOrDefault(o => o.CustomerName == "Customer Two");

            Assert.NotNull(retrievedOrder1);
            Assert.NotNull(retrievedOrder2);
            Assert.NotNull(retrievedOrder1.OrderStatus);
            Assert.NotNull(retrievedOrder1.ShippingMethod);
            Assert.NotNull(retrievedOrder2.OrderStatus);
            Assert.NotNull(retrievedOrder2.ShippingMethod);
        }
    }
}
