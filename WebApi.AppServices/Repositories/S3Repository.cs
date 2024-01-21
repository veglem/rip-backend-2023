using Minio.DataModel.Args;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class S3Repository : IS3Repository
{
    private S3Context _context;

    public S3Repository(S3Context context)
    {
        _context = context;
    }
    
    public async Task AddImage(Stream image, string name, CancellationToken cancellationToken)
    {
        PutObjectArgs args = new PutObjectArgs().WithBucket("images").WithObject(name + ".jpg").WithObjectSize(image.Length).WithStreamData(image);
        
        await _context.AddImage(args, cancellationToken);
    }

    public async Task<byte[]> GetImage(string fileName,
        CancellationToken cancellationToken)
    {
        byte[] returnStream = new byte[30000000];
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;
        Task t = Task.Delay(10000, token);
        GetObjectArgs args = new GetObjectArgs().WithBucket("images")
            .WithObject(fileName).WithCallbackStream(async stream =>
            {
                Console.WriteLine(stream.Read(returnStream));
                source.Cancel(); 
            });

        await _context.GetImage(args, cancellationToken);
        try
        {
            await t;
        }
        catch (TaskCanceledException)
        {
            
        }
        
        return returnStream;
    }

    // public Task<byte[]> GetImage(string name, CancellationToken cancellationToken)
    // {
    //     _context.
    // }
}
