using POCArgos.Models;

namespace Test.Tests.Mocks
{
    internal class OrdersMock
    {
        /// <summary>
        /// Crea una orden de prueba básica
        /// </summary>
        public Order CreateBasicOrder(string? customerName = null, int? statusId = null, int? shippingId = null, decimal? amount = null)
        {
            return new Order
            {
                CustomerName = customerName ?? "Test Customer",
                OrderStatusId = statusId ?? 1,
                ShippingMethodId = shippingId ?? 1,
                TotalAmount = amount ?? 150.00m
            };
        }

        /// <summary>
        /// Crea una orden con datos específicos para pruebas
        /// </summary>
        public Order CreateOrderForTest(string customerName, int statusId, int shippingId, decimal totalAmount)
        {
            return new Order
            {
                CustomerName = customerName,
                OrderStatusId = statusId,
                ShippingMethodId = shippingId,
                TotalAmount = totalAmount
            };
        }

        /// <summary>
        /// Orden de prueba: "John Smith" con estado Processing y Express shipping
        /// </summary>
        public Order CreateJohnSmithOrder() => CreateOrderForTest("John Smith", 2, 2, 250.50m);


        /// <summary>
        /// Orden de prueba: actualización de nombre
        /// </summary>
        public Order CreateOrderForUpdate() => CreateOrderForTest("Original Name", 1, 1, 100.00m);

        /// <summary>
        /// Orden con cliente "Customer One"
        /// </summary>
        public Order CreateCustomerOneOrder() => CreateOrderForTest("Customer One", 1, 1, 100.00m);

        /// <summary>
        /// Orden con cliente "Customer Two"
        /// </summary>
        public Order CreateCustomerTwoOrder() => CreateOrderForTest("Customer Two", 2, 2, 200.00m);

    }
}
