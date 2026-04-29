# Tests de Integración - POCArgos

Tests de integración con XUnit conectados a la base de datos real de SQL Server.

## 📋 Descripción

Los tests se ejecutan contra la base de datos real configurada en `appsettings.json`:
- **Servidor**: PruebaArgos.mssql.somee.com
- **Base de datos**: PruebaArgos
- **Tipo**: Integración (base de datos real)

## 📁 Estructura

```
Test/
├── appsettings.json                           # Configuración DB real
├── Fixtures/
│   └── IntegrationTestFixture.cs             # Fixture de conexión real
├── Tests/
│   ├── Integration/
│   │   ├── CatalogsIntegrationTests.cs       # Tests del servicio Catalogs
│   │   └── OrdersIntegrationTests.cs         # Tests del servicio Orders
│   └── UnitTest1.cs                          # Prueba básica
└── README.md
```

## 🔧 Configuración

### Dependencias

- **Microsoft.EntityFrameworkCore.SqlServer** 8.0.26 - Acceso a SQL Server
- **Microsoft.Extensions.Configuration.Json** - Lectura de appsettings.json
- **xunit** - Framework de testing
- **Moq** - Para mocking futuro

### Fixture de Integración

`IntegrationTestFixture.cs` maneja:
1. Lectura de `appsettings.json`
2. Conexión a la BD real vía EntityFrameworkCore
3. Migración automática de esquema
4. Limpieza de recursos

```csharp
var fixture = new IntegrationTestFixture();
await fixture.InitializeAsync();
var dbContext = fixture.DbContext;
```

## ✅ Tests Implementados

### CatalogsIntegrationTests (10 tests)

Verifica que los catálogos (estados y métodos de envío) existan en la BD:

- `RetrieveAllOrderStatusesFromDatabase` - Valida existencia de estados
- `IncludePendingStatusInResults` - Pending (Id=1)
- `IncludeProcessingStatusInResults` - Processing (Id=2)
- `IncludeShippedStatusInResults` - Shipped (Id=3)
- `IncludeDeliveredStatusInResults` - Delivered (Id=4)
- `RetrieveAllShippingMethodsFromDatabase` - Valida métodos
- `IncludeStandardShippingMethod` - Standard (Id=1)
- `IncludeExpressShippingMethod` - Express (Id=2)
- `IncludeNextDayShippingMethod` - Next Day (Id=3)

### OrdersIntegrationTests (11 tests)

Pruebas CRUD completas contra BD real:

**Creación**
- `CreateNewOrderInDatabase` - Inserta orden
- `HandleDecimalAmountsWithPrecision` - Valida decimales

**Lectura**
- `RetrieveCreatedOrderById` - GetById funciona
- `ReturnNullWhenOrderDoesNotExist` - Maneja no existencia
- `IncludeOrderStatusWhenRetrievingOrder` - Status incluido
- `IncludeShippingMethodWhenRetrievingOrder` - Método incluido
- `RetrieveMultipleOrdersWithAllRelations` - GetAll con relaciones

**Actualización**
- `UpdateOrderSuccessfully` - Actualiza datos
- `ThrowExceptionWhenUpdatingOrderWithMismatchedId` - Validación
- `ReturnFalseWhenUpdatingNonExistentOrder` - Manejo error
- `UpdateAllOrderFields` - Actualización múltiple

## 🚀 Ejecución

### Todos los tests

```bash
dotnet test Test
```

### Con más detalles

```bash
dotnet test Test --logger "console;verbosity=detailed"
```

### Solo tests de Catalogs

```bash
dotnet test Test --filter "CatalogsIntegration"
```

### Solo tests de Orders

```bash
dotnet test Test --filter "OrdersIntegration"
```

## 📊 Resultados

```
Total tests:   21
Passed:        21 ✅
Failed:        0
Duration:      ~25 segundos
```

## ⚠️ Notas Importantes

1. **Los tests modifican la BD**: Los tests crean/actualizan registros en la BD real
2. **Conexión requerida**: Necesita acceso a `PruebaArgos.mssql.somee.com`
3. **Independencia**: Cada test es independiente y puede ejecutarse en cualquier orden
4. **Relaciones**: Los tests validan que las relaciones (OrderStatus, ShippingMethod) se incluyan correctamente
5. **Código legible**: Nombres en PascalCase, sin guiones, fácil de leer y mantener

## 🔍 Ejemplo de Test

```csharp
[Fact]
public async Task CreateNewOrderInDatabase()
{
    var newOrder = new Order
    {
        CustomerName = "Test Customer",
        OrderStatusId = 1,
        ShippingMethodId = 1,
        TotalAmount = 150.00m
    };

    var createdOrder = await _ordersService.CreateOrderAsync(newOrder);

    Assert.NotNull(createdOrder);
    Assert.True(createdOrder.Id > 0);
    Assert.Equal("Test Customer", createdOrder.CustomerName);
}
```

Código claro, directo y mantenible.
