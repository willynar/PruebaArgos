using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using POCArgos.Interfaces;
using POCArgos.Models;

namespace POCArgos.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Controlador encargado de proveer los datos de los catálogos (estados y métodos de envío).
/// </summary>
public class CatalogsController : ControllerBase
{
    private readonly ICatalogsService _catalogsService;

    /// <summary>
    /// Constructor del controlador de catálogos.
    /// </summary>
    /// <param name="catalogsService">Servicio que maneja la lógica de los catálogos en caché.</param>
    public CatalogsController(ICatalogsService catalogsService)
    {
        _catalogsService = catalogsService;
    }

    /// <summary>
    /// Consulta la lista de estados que puede tener un pedido.
    /// </summary>
    [OutputCache(PolicyName = "OrderStatuses")]
    [HttpGet("order/status")]
    public async Task<IActionResult> GetOrderStatuses()
    {
        try
        {
            var statuses = await _catalogsService.GetOrderStatusesAsync();
            return Ok(statuses);
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
    /// Consulta la lista de métodos de envío disponibles.
    /// </summary>
    [OutputCache(PolicyName = "ShippingMethods")]
    [HttpGet("shipping/methods")]
    public async Task<IActionResult> GetShippingMethods()
    {
        try
        {
            var methods = await _catalogsService.GetShippingMethodsAsync();
            return Ok(methods);
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
    /// Limpia el caché de los catálogos (estados y métodos de envío).
    /// </summary>
    [HttpPost("clear/cache")]
    public async Task<IActionResult> ClearCache([FromServices] IOutputCacheStore cacheStore)
    {
        try
        {
            await cacheStore.EvictByTagAsync("OrderStatusesTag", default);
            await cacheStore.EvictByTagAsync("ShippingMethodsTag", default);
            
            return Ok(new { message = "Caché de catálogos limpiado correctamente." });
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
