using Grpc.Core;

namespace GrpcServer.Services;

public class CalculatorService : GrpcServer.CalculatorService.CalculatorServiceBase
{
    public override Task<CalculatorResponse> Add(CalculatorRequest request, ServerCallContext context)
    {
        var newAddition = new CalculatorResponse
        {
            Operand1 = request.Operand1,
            Operand2 = request.Operand2,
            Operation = "Addition",
            Result = request.Operand1 + request.Operand2
        };

        return Task.FromResult(newAddition);
    }

    public override Task<CalculatorResponse> Subtract(CalculatorRequest request, ServerCallContext context)
    {
        var newSubtraction = new CalculatorResponse
        {
            Operand1 = request.Operand1,
            Operand2 = request.Operand2,
            Operation = "Subtraction",
            Result = request.Operand1 - request.Operand2
        };

        return Task.FromResult(newSubtraction);
    }

    public override Task<CalculatorResponse> Multiply(CalculatorRequest request, ServerCallContext context)
    {
        var newMultiplication = new CalculatorResponse
        {
            Operand1 = request.Operand1,
            Operand2 = request.Operand2,
            Operation = "Multiplication",
            Result = request.Operand1 * request.Operand2
        };

        return Task.FromResult(newMultiplication);
    }

    public override Task<CalculatorResponse> Divide(CalculatorRequest request, ServerCallContext context)
    {
        if (request.Operand2 == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Cannot divide by zero."));
        }

        var newDivision = new CalculatorResponse
        {
            Operand1 = request.Operand1,
            Operand2 = request.Operand2,
            Operation = "Division",
            Result = (double)request.Operand1 / request.Operand2
        };

        return Task.FromResult(newDivision);
    }
}
