using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Repositories;

public interface IUnitRepository
{
    public Task<List<GetUnitResult>> GetAllUnits(
        string filter,
        CancellationToken cancellationToken);

    public Task<GetUnitResult?> GetUnitById(
        CancellationToken cancellationToken,
        int id);
    
    public Task<int> AddUnit(
        CancellationToken cancellationToken,
        NewUnit unit);
    
    public Task UpdateUnit(
        CancellationToken cancellationToken,
        int id,
        NewUnit unit);
    
    public Task<List<GetUnitResult>> GetAllUnitsWithDeleted(
        CancellationToken cancellationToken);
    
    public Task LogicUnitDelete(
        CancellationToken cancellationToken,
        int id);
}
