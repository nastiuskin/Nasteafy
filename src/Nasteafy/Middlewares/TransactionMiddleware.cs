using Nasteafy.Application.Common.Abstractions.Data;

namespace Nasteafy.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // It is better to move that into Pipeline behavior and wrap transactions around ITransactionalCommand. 
        // Things like global logging, http requests, authorization and authentication, or global exception handling can be made as middlewares
        // Things that are a part of application specific concerns like validation, transactions, queries and command are better to be moved to pipeline behaviors. In your case it is this class.

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                await _next(context);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
