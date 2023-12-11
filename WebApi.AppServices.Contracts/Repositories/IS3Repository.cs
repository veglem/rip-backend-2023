namespace WebApi.AppServices.Contracts.Repositories;

public interface IS3Repository
{
    public Task AddImage(Stream image, string name, CancellationToken cancellationToken);

    // public Task<byte[]> GetImage(string name,
    //     CancellationToken cancellationToken);
}
