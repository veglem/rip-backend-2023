using System.Net;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Services.Validators;
using WebApi.AppServices.Models;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/units")]
public class UnitController : Controller
{
    private IUnitHandler _unitHandler;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitHandler"></param>
    public UnitController(IUnitHandler unitHandler)
    {
        _unitHandler = unitHandler;
    }

    /// <summary>
    /// Возвращает список подразделений МГТУ им. Баумана.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ICollection<GetUnitResult>> GetUniversityUnits(
        CancellationToken cancellationToken,
        [FromQuery]string filter = "")
    {
        return await _unitHandler.GetUnits(filter, cancellationToken);
    }

    /// <summary>
    /// Возвращает подразделение МГТУ им. Баумана по его id
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
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
    [HttpPut("{id:int}/update")]
    public async Task<GetUnitResult> UpdateUnit(
        CancellationToken cancellationToken,
        [FromRoute]int id,
        [FromBody]NewUnit unit)
    {
        try
        {
            await _unitHandler.UpdateUnit(cancellationToken, id, unit);

            GetUnitResult? updetedUnit =
                await _unitHandler.GetUnitById(cancellationToken, id);

            if (updetedUnit is null)
            {
                await Results.BadRequest("ошибка обновления").ExecuteAsync(HttpContext);
            }

            return updetedUnit;
        }
        catch (ArgumentNullException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Добавляет пустое подразделение, возвращает список всех подразделений
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<ICollection<GetUnitResult>> AddNewUnit(
        CancellationToken cancellationToken)
    {
        NewUnit unit = new NewUnit()
        {
            Name = "Имя по умолчанию",
            Description = "Описание по умолчанию",
            ImgUrl = "images/default",
            IsDeleted = true
        };

        try
        {
            await _unitHandler.AddNewUnit(cancellationToken, unit);
        }
        catch (ValidationException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
            return new List<GetUnitResult>();
        }

        return await _unitHandler.GetUnits("", cancellationToken);
    }
    
    /// <summary>
    /// Возвращает все подразделения МГТУ им. Баумана, включая удаленные
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("all")]
    public async Task<ICollection<GetUnitResult>> GetUniversityUnitsWithDeleted(
        CancellationToken cancellationToken)
    {
        return await _unitHandler.GetUnitsWithDeleted(cancellationToken);
    }

    /// <summary>
    /// Удаляет подразделение МГТУ им. Баумана.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}/delete")]
    public async Task<ICollection<GetUnitResult>> LogicDeleteUnit(
        CancellationToken cancellationToken, 
        [FromRoute]int id)
    {
        try
        {
            await _unitHandler.UnitLogicDelete(cancellationToken, id);

            return await _unitHandler.GetUnits("", cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Обновляет изображение для подразделения МГТУ им. Баумана
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    /// <param name="image"></param>
    [HttpPut("{id}/image")]
    public async Task AddImage(
        CancellationToken cancellationToken,
        int id,
        IFormFile image)
    {
        try
        {
            await _unitHandler.AddImage(image.OpenReadStream(), id, cancellationToken);
        } catch (ArgumentNullException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="id"></param>
    [HttpGet("{id:int}/image")]
    public async Task GetImage(
        CancellationToken cancellationToken,
        int id)
    {
        var unit = await _unitHandler.GetUnitById(cancellationToken, id);
        
        await Results.Redirect("http://localhost:9000/" + HttpUtility.UrlPathEncode(unit.ImgUrl)).ExecuteAsync(HttpContext);
    }
    
    
}
