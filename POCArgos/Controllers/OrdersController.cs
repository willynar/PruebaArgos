using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POCArgos.Interfaces;
using POCArgos.Models;

namespace POCArgos.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Controlador que gestiona toda la información relacionada con los pedidos.
/// </summary>
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Constructor del controlador de pedidos.
    /// </summary>
    /// <param name="orderService">Servicio que contiene la lógica de negocio de los pedidos.</param>
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Trae todos los pedidos que están guardados en el sistema.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
        catch (Exception exc)
        {
            string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
            return StatusCode(StatusCodes.Status500InternalServerError, new StsResponse
            {
                Message = "Error! " + ErrorMsg
            });
        }
    }

    /// <summary>
    /// Busca y devuelve la información de un solo pedido usando su número de ID.
    /// </summary>
    /// <param name="id">El identificador único del pedido que se desea consultar.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        catch (Exception exc)
        {
            string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
            return StatusCode(StatusCodes.Status500InternalServerError, new StsResponse
            {
                Message = "Error! " + ErrorMsg
            });
        }
    }

    /// <summary>
    /// Recibe los datos de un pedido nuevo y lo guarda.
    /// </summary>
    /// <param name="order">Objeto con la información del pedido a crear.</param>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        try
        {
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception exc)
        {
            string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
            return StatusCode(StatusCodes.Status500InternalServerError, new StsResponse
            {
                Message = "Error! " + ErrorMsg
            });
        }
    }

    /// <summary>
    /// Actualiza los datos de un pedido. Verifica que nadie más lo haya modificado antes de guardar.
    /// </summary>
    /// <param name="id">El identificador único del pedido a actualizar.</param>
    /// <param name="order">Objeto con la nueva información del pedido.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] Order order)
    {
        try
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var success = await _orderService.UpdateOrderAsync(id, order);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "El pedido fue modificado por otro usuario. Por favor, recargue los datos." });
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (Exception exc)
        {
            string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
            return StatusCode(StatusCodes.Status500InternalServerError, new StsResponse
            {
                Message = "Error! " + ErrorMsg
            });
        }
    }
}
