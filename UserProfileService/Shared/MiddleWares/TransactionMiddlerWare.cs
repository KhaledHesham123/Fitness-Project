using UserProfileService.Contract;

namespace UserProfileService.Shared.MiddleWares
{
    public class TransactionMiddlerWare : IMiddleware
    {
        private readonly IunitofWork unitofWork;
        private readonly ILogger<TransactionMiddlerWare> logger;

        public TransactionMiddlerWare(IunitofWork unitofWork,ILogger<TransactionMiddlerWare> logger)
        {
            this.unitofWork = unitofWork;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.Request.Method == HttpMethods.Get)
                {
                   await next(context);
                }
                else 
                {
                    await unitofWork.BeginTransactionAsync();
                    await next(context);
                    await unitofWork.CommitTransactionAsync();
                }

            }
            catch (Exception ex)
            {
                await unitofWork.RollbackTransactionAsync();

                logger.LogError( ex, "An error occurred during transaction processing.");

            }

        }
    }
}
