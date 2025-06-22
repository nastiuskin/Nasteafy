using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ResultFailureLoggingBehavior<TRequest, TResponse>(
    ILogger<ResultFailureLoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (response is Result result && result.IsFailed)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Message));
            logger.LogWarning("Request {RequestType} failed with errors: {Errors}", typeof(TRequest).Name, errors);
        }

        return response;
    }
}
