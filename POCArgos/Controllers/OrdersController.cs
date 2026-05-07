using Microsoft.AspNetCore.Mvc;
using POCArgos.Interfaces;
using POCArgos.DTOs;

namespace POCArgos.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Controlador que gestiona toda la información relacionada con los pedidos.
/// </summary>
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

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
            return HandleError(exc);
        }
    }

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
            return HandleError(exc);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderUpsertDto orderDto)
    {
        try
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception exc)
        {
            return HandleError(exc);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] OrderUpsertDto orderDto)
    {
        try
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }

            var success = await _orderService.UpdateOrderAsync(id, orderDto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
        catch (Exception exc)
        {
            return HandleError(exc);
        }
    }

    private IActionResult HandleError(Exception exc)
    {
        string ErrorMsg = exc.GetBaseException().InnerException != null ? exc.GetBaseException().InnerException.Message : exc.GetBaseException().Message;
        return StatusCode(StatusCodes.Status500InternalServerError, new StsResponse
        {
            Message = "Error! " + ErrorMsg
        });
    }
}
