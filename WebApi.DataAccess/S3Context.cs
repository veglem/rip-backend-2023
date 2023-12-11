using Minio;
using Minio.DataModel.Args;

namespace WebApi.DataAccess;

public class S3Context
{
    private IMinioClient _client;

    public S3Context(IMinioClient client)
    {
        _client = client;
    }

    public async Task AddImage(PutObjectArgs args, CancellationToken cancellationToken)
    {
        await _client.PutObjectAsync(args, cancellationToken);
    }

    // public async Task<byte[]> GetImage(string name,
    //     CancellationToken cancellationToken)
    // {
    //     GetObjectArgs getObjectArgs = new GetObjectArgs()
    //         .WithBucket("images")
    //         .WithObject("myobject")
    //         .WithCallbackStream((stream) =>
    //         {
    //             List<byte> array = new List<byte>();
    //             for (int i = 0; i < stream.Length; ++i)
    //             {
    //                 array.Add((byte)stream.ReadByte());
    //                 return array.ToArray();
    //             }
    //             stream.CopyTo(Console.OpenStandardOutput());
    //         });
    //     
    //     (await _client.GetObjectAsync(new GetObjectArgs().WithBucket("images")
    //         .)).
    // }
}
