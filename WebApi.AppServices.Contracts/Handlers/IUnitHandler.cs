using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Handlers;

public interface IUnitHandler
{
    public Task<List<GetUnitResult>> GetUnits(
        string filter,
        CancellationToken cancellationToken);

    public Task<GetUnitResult?> GetUnitById(
        CancellationToken cancellationToken,
        int id);
    
    public Task<int> AddNewUnit(
            CancellationToken cancellationToken,
            NewUnit unit);
    
    public Task UpdateUnit(
        CancellationToken cancellationToken,
        int id,
        NewUnit unit);
    
    public Task<List<GetUnitResult>> GetUnitsWithDeleted(
        CancellationToken cancellationToken);

    public Task UnitLogicDelete(CancellationToken cancellationToken, int id);

    public Task AddImage(Stream image, int id, CancellationToken cancellationToken);

    // public Task<byte[]> GetImage(int id, CancellationToken cancellationToken);
}
