using FluentResults;
using Microsoft.Extensions.Logging;

public class ResultLogger : IResultLogger
{
    private readonly ILoggerFactory _loggerFactory;

    public ResultLogger(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public void Log(string context, string content, ResultBase result, LogLevel logLevel)
    {
        var logger = _loggerFactory.CreateLogger(context ?? "FluentResults");
        var reasons = string.Join("; ", result.Reasons.Select(r => r.Message));
        logger.Log(logLevel, "{Context}: {Reasons}", context, reasons);
    }

    public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
    {
        var logger = _loggerFactory.CreateLogger(typeof(TContext));
        var reasons = string.Join("; ", result.Reasons.Select(r => r.Message));
        logger.Log(logLevel,
          "Operation failed: {Content}. Reasons: {Reasons}",
          content ?? string.Empty,
          reasons);
    }
}
