using Grpc.Core;

namespace GrpcServer.Services;

public class CheckedSquaredNumbers : GrpcServer.CheckSquaredNumbers.CheckSquaredNumbersBase
{

    public override async Task Check(
        IAsyncStreamReader<NumbersItem> requestStream,
        IServerStreamWriter<ServerResponse> responseStream,
        ServerCallContext context
    )
    {
        await foreach (var item in requestStream.ReadAllAsync())
        {
           if ((item.A * item.A) == item.B) {
                await responseStream.WriteAsync(new ServerResponse
                {
                    Result = $"Number {item.A} is squared to {item.B}"
                });
            } else
            {
                await responseStream.WriteAsync(new ServerResponse {
                    Result = $"Number {item.A} is not squared to {item.B}"
                });
            }
        }
    }
}
