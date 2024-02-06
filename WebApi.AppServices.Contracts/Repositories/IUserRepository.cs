using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAndPassword(string username,
        string password, CancellationToken cancellationToken);
    
    public Task<User> AddUser(string username,
        string password, CancellationToken cancellationToken);

    public  Task UpdateUserInfo(
        string username,
        UpdateUserInfo userInfo,
        CancellationToken cancellationToken);

    public Task<GetUserInfo> GetUserInfo(
        string username,
        CancellationToken cancellationToken);
    
    public Task UpdateImage(string username, string imgPath,
        CancellationToken cancellationToken);

    public Task<UserCredentials?> GetUserCreds(string username,
        CancellationToken cancellationToken);
}
