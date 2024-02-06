using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Services.Validators;
using WebApi.AppServices.Exceptions;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/units")]
public class UnitController : Controller
{
    private IUnitHandler _unitHandler;
    private IOrdersHandler _ordersHandler;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitHandler"></param>
    public UnitController(IUnitHandler unitHandler, IOrdersHandler ordersHandler, IDistributedCache cache)
    {
        _unitHandler = unitHandler;
        _ordersHandler = ordersHandler;
        _cache = cache;
    }

    /// <summary>
    /// Возвращает список подразделений МГТУ им. Баумана.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<GetAllUnitsResult> GetUniversityUnits(
        CancellationToken cancellationToken,
        [FromQuery]string filter = "")
    {
        GetAllUnitsResult res = new GetAllUnitsResult()
        {
            Units = await _unitHandler.GetUnits(filter, cancellationToken),
            Draft = null
        };
        
        if (User.Identity is not null)
        {
            var o = await _ordersHandler.GetUserOrders(User.Identity.Name, cancellationToken);

            res.Draft = o.FirstOrDefault(order => order.Status == 1)?.Id;
        }
        
        return res;
    }

    /// <summary>
    /// Возвращает подразделение МГТУ им. Баумана по его id
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Успешное получение подразделения</response>
    /// <response code="404">Искомый id не существует</response>
    [HttpGet("{id:int}")]
    public async Task<GetUnitResult> GetUniversityUnitById(
        CancellationToken cancellationToken, int id)
    {
        GetUnitResult? unit =
            await _unitHandler.GetUnitById(cancellationToken, id);

        if (unit is null)
        {
            await Results.NotFound().ExecuteAsync(HttpContext);
        }

        return unit;
    }

    /// <summary>
    /// Обновляет информацию о подразделении, возвращает обновленное подразделение
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    /// <response code="200">Успешное обновление подразделения</response>
    /// <response code="400">Ошибка обновления</response>
    [HttpPut("{id:int}/update")]
    [Authorize(Roles = "moderator")]
    public async Task<GetUnitResult> UpdateUnit(
        CancellationToken cancellationToken,
        [FromRoute]int id,
        [FromBody]NewUnit unit)
    {
        try
        {
            if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
            {
                await Results.Forbid().ExecuteAsync(HttpContext);
                return null;
            }
            
            await _unitHandler.UpdateUnit(cancellationToken, id, unit);

            GetUnitResult? updetedUnit =
                await _unitHandler.GetUnitById(cancellationToken, id);

            if (updetedUnit is null)
            {
                await Results.BadRequest("ошибка обновления").ExecuteAsync(HttpContext);
                return null;
            }

            return updetedUnit;
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Добавляет пустое подразделение, возвращает список всех подразделений
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное добавление пустого подразделения</response>
    /// <response code="400">Ошибка создания подразделения</response>
    [HttpPost("create")]
    [Authorize(Roles = "moderator")]
    public async Task<ICollection<GetUnitResult>> AddNewUnit(
        CancellationToken cancellationToken)
    {
        if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        NewUnit unit = new NewUnit()
        {
            Name = "Имя по умолчанию",
            Description = "Описание по умолчанию",
            ImgUrl = "images/default.png",
            IsDeleted = true
        };

        try
        {
            await _unitHandler.AddNewUnit(cancellationToken, unit);
        }
        catch (ValidationException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
            return new List<GetUnitResult>();
        }

        return await _unitHandler.GetUnits("", cancellationToken);
    }
    
    /// <summary>
    /// Возвращает все подразделения МГТУ им. Баумана, включая удаленные
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное получение подразделений</response>
    [HttpGet("all")]
    [Authorize(Roles = "moderator")]
    public async Task<ICollection<GetUnitResult>> GetUniversityUnitsWithDeleted(
        CancellationToken cancellationToken)
    {
        if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        return await _unitHandler.GetUnitsWithDeleted(cancellationToken);
    }

    /// <summary>
    /// Удаляет подразделение МГТУ им. Баумана.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200">Успешное удаление подразделения</response>
    /// <response code="400">Ошибка удаления</response>
    [HttpDelete("{id:int}/delete")]
    [Authorize(Roles = "moderator")]
    public async Task<ICollection<GetUnitResult>> LogicDeleteUnit(
        CancellationToken cancellationToken, 
        [FromRoute]int id)
    {
        if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        try
        {
            await _unitHandler.UnitLogicDelete(cancellationToken, id);

            return await _unitHandler.GetUnits("", cancellationToken);
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Обновляет изображение для подразделения МГТУ им. Баумана
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <param name="image"></param>
    /// <response code="200">Успешное обновление изображения</response>
    /// <response code="400">Ошибка обновление изображения</response>
    [HttpPut("{id}/image")]
    [Authorize(Roles = "moderator")]
    public async Task AddImage(
        CancellationToken cancellationToken,
        int id,
        IFormFile image)
    {
        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return;
        }
        
        try
        {
            await _unitHandler.AddImage(image.OpenReadStream(), id, cancellationToken);
        } catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }
    }

    /// <summary>
    /// Получение изображения подразделения
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    [HttpGet("{id:int}/image")]
    public async Task GetImage(
        CancellationToken cancellationToken,
        int id)
    {
        var unit = await _unitHandler.GetUnitById(cancellationToken, id);
        
        await Results.Redirect("http://loacalhost:9000/" + HttpUtility.UrlPathEncode(unit.ImgUrl)).ExecuteAsync(HttpContext);
    }

    /// <summary>
    /// Добавление подразделения к приказу
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное добавление подразделения</response>
    /// <response code="400">Ошибка добавления</response>
    /// <response code="403">Пользователь не авторизован</response>
    [HttpPost("{id:int}/add_to_order")]
    [Authorize(Roles = "user")]
    public async Task<GetUnitResult> AddUnitToOrder(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ?? string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            var result = await _ordersHandler.AddUnitToOrder(id,
                User.Identity.Name,
                cancellationToken);

            return result;
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }

        return null;
    }
}


