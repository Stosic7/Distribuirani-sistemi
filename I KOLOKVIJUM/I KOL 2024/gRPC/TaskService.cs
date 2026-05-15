using Grpc.Core;

namespace GrpcServer.Services;

public class TaskService:GrpcServer.TaskService.TaskServiceBase
{
    private static readonly List<TaskItem> _tasks = new List<TaskItem>(); 
    private static int _nextId = 1;

    // implementacija AddTask metode
    public override Task<AddTaskResponse> AddTask(AddTaskRequest request, ServerCallContext context)
    {
        var task = new TaskItem
        {
            Id = _nextId++,
            Title = request.Title,
            IsCompleted = false
        };

        _tasks.Add(task);

        Console.WriteLine($"Dodat zadatak: [{task.Id}] {task.Title}");
        return Task.FromResult(new AddTaskResponse { Task = task });
    }

    // Implementacija ListTasks metode
    public override Task<ListTasksResponse> ListTasks(
        ListTasksRequest request, ServerCallContext context)
    {
        Console.WriteLine("Klijent trazi listu zadataka...");

        var response = new ListTasksResponse();
        response.Tasks.AddRange(_tasks);

        return Task.FromResult(response);
    }

    // Implementacija MarkTaskAsCompleted metode
    public override Task<MarkTaskAsCompletedResponse> MarkTaskAsCompleted(
        MarkTaskAsCompletedRequest request, ServerCallContext context)
    {
        Console.WriteLine($"Trazim zadatak sa ID: {request.Id}");

        var task = _tasks.FirstOrDefault(t => t.Id == request.Id);

        if (task == null)
        {
            throw new RpcException(new Status(
                StatusCode.NotFound,
                $"Zadatak sa ID {request.Id} nije pronadjen"));
        }

        task.IsCompleted = true;
        Console.WriteLine($"Zadatak [{task.Id}] oznacen kao zavrsen.");

        return Task.FromResult(new MarkTaskAsCompletedResponse { Task = task });
    }
}
