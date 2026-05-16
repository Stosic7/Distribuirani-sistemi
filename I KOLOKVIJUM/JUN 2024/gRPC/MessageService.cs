using Grpc.Core;

namespace GrpcServer.Services;

public class MessageService : GrpcServer.MessageService.MessageServiceBase
{
    private static readonly List<MessageItem> _messages = new List<MessageItem>();
    private static int _nextId = 1;

    public override Task<SendMessageResponse> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        var message = new MessageItem
        {
            Id = _nextId++,
            Content = request.Content
        };

        _messages.Add(message);

        return Task.FromResult(new SendMessageResponse {Message = message});
    }

    public override Task<DeleteMessageResponse> DeleteMessage(DeleteMessageRequest request, ServerCallContext context)
    {
        var message = _messages.FirstOrDefault(m => m.Id == request.Id);
        if (message == null) {throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Poruka sa ID {request.Id} nije pronadjena"));}
        
        _messages.Remove(message);

        return Task.FromResult(new DeleteMessageResponse { Success = true });
    }

    public override async Task ListMessages(
        ListMessagesRequest request,
        IServerStreamWriter<MessageItem> responseStream,
        ServerCallContext context
    )
    {
        foreach (var message in _messages)
        {
            await responseStream.WriteAsync(message);
        }
    }
}
