using Microsoft.AspNetCore.Mvc;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.Controllers;

/// <summary>
/// 
/// </summary>
[Route("units")]
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
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ICollection<GetUnitResult>> GetUniversityUnits(
        CancellationToken cancellationToken)
    {
        return await _unitHandler.GetUnits(cancellationToken);
    }
}
