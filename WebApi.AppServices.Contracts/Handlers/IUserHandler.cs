using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Handlers;

public interface IUserHandler
{
    public Task<User?> GetUserByLoginAndPass(string username,
        string password,
        CancellationToken cancellationToken);
    
    public Task<User> AddUser(string username,
        string password,
        CancellationToken cancellationToken);
}
