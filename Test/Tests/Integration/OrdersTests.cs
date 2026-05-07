using POCArgos.Services;
using POCArgos.Domain.Entities;
using POCArgos.DTOs;
using POCArgos.Data.Repositories;
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
            var orderRepository = new OrderRepository(_fixture.DbContext);
            _ordersService = new Orders(orderRepository);
        }

        public async Task DisposeAsync()
        {
            await _fixture.DisposeAsync();
        }

        [Fact]
        public async Task CreateNewOrderInDatabase()
        {
            var newOrder = OrdersMock.CreateBasicOrder();
            var upsertDto = MapToUpsertDto(newOrder);

            var createdOrder = await _ordersService.CreateOrderAsync(upsertDto);

            Assert.NotNull(createdOrder);
            Assert.True(createdOrder.Id > 0);
            Assert.Equal("Test Customer", createdOrder.CustomerName);
            Assert.Equal(150.00m, createdOrder.TotalAmount);
        }

        private OrderUpsertDto MapToUpsertDto(Order o) => new OrderUpsertDto
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            OrderStatusId = o.OrderStatusId,
            ShippingMethodId = o.ShippingMethodId,
            DeliveryAddress = o.DeliveryAddress,
            TotalAmount = o.TotalAmount,
            InternalComent = o.InternalComent,
            RowVersion = o.RowVersion
        };

        [Fact]
        public async Task RetrieveCreatedOrderById()
        {
            var newOrder = OrdersMock.CreateJohnSmithOrder();

            var created = await _ordersService.CreateOrderAsync(MapToUpsertDto(newOrder));
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

            var created = await _ordersService.CreateOrderAsync(MapToUpsertDto(initialOrder));

            var upsertDto = new OrderUpsertDto
            {
                Id = created.Id,
                CustomerName = "Updated Name",
                TotalAmount = 200.00m,
                OrderStatusId = 2,
                ShippingMethodId = created.ShippingMethodId,
                RowVersion = created.RowVersion
            };

            var updated = await _ordersService.UpdateOrderAsync(created.Id, upsertDto);

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

            await _ordersService.CreateOrderAsync(MapToUpsertDto(order1));
            await _ordersService.CreateOrderAsync(MapToUpsertDto(order2));

            var allOrders = await _ordersService.GetOrdersAsync();

            var retrievedOrder1 = allOrders.FirstOrDefault(o => o.CustomerName == "Customer One");
            var retrievedOrder2 = allOrders.FirstOrDefault(o => o.CustomerName == "Customer Two");

            Assert.NotNull(retrievedOrder1);
            Assert.NotNull(retrievedOrder2);
            Assert.NotNull(retrievedOrder1.OrderStatusName);
            Assert.NotNull(retrievedOrder1.ShippingMethodName);
            Assert.NotNull(retrievedOrder2.OrderStatusName);
            Assert.NotNull(retrievedOrder2.ShippingMethodName);
        }
    }
}
