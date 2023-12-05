using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Handlers;

public interface IUnitHandler
{
    public Task<List<GetUnitResult>> GetUnits(CancellationToken cancellationToken);
}
