using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Repositories;

public interface IUnitRepository
{
    public Task<List<GetUnitResult>> GetAllUnits(CancellationToken cancellationToken);
}
