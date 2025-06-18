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

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            //if (context.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    await _next(context);
            //    return;
            //}

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
