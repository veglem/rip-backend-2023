using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.DataAccess;

namespace WebApi.AppServices.Handlers;

public class UserHandler : IUserHandler
{
    private readonly IUserRepository _userRepository;

    private readonly IS3Repository _s3Repository;

    public UserHandler(IUserRepository userRepository, IS3Repository s3Repository)
    {
        _userRepository = userRepository;
        _s3Repository = s3Repository;
    }

    public async Task<User?> GetUserByLoginAndPass(string username, string password,
        CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByUsernameAndPassword(username,
            password, cancellationToken);
    }

    public async Task<User> AddUser(string username, string password,
        CancellationToken cancellationToken)
    {
        return await _userRepository.AddUser(username, password, cancellationToken);
    }

    public async Task UpdateUserInfo(
        string username,
        UpdateUserInfo userInfo,
        CancellationToken cancellationToken)
    {
        await _userRepository.UpdateUserInfo(username, userInfo, cancellationToken);
    }

    public async Task<GetUserInfo> GetUserInfo(string username, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserInfo(username, cancellationToken);
    }

    public async Task AddImageProfile(Stream image, string username,
        CancellationToken cancellationToken)
    {
        Contracts.Models.Responce.GetUserInfo user = await 
            _userRepository.GetUserInfo(username, cancellationToken);

        await _s3Repository.AddImage(image, username + user.GetHashCode().ToString(),
            cancellationToken);

        await _userRepository.UpdateImage(
            username,
            $"images/{username + user.GetHashCode()}.jpg", 
            cancellationToken);
    }

    public async Task<UserCredentials?> GetUserCreds(string username,
        CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserCreds(username, cancellationToken);
    }
}
