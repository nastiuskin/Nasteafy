using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

public class ResultFailureLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ResultFailureLoggingBehavior<TRequest, TResponse>> _logger;

    public ResultFailureLoggingBehavior(ILogger<ResultFailureLoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (response is Result result && result.IsFailed)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Message));
            _logger.LogWarning("Request {RequestType} failed with errors: {Errors}", typeof(TRequest).Name, errors);
        }

        return response;
    }
}