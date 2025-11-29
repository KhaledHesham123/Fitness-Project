using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("logainPolicy", context =>
                {
                    var userid = context.User.Identity?.Name ??
                      context.Request.Headers["X-Api-Key"].FirstOrDefault() ?? "anonymous";
                    var key = $"{userid}|login";

                    return RateLimitPartition.GetTokenBucketLimiter(key, _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 5,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0,
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        TokensPerPeriod = 5,
                        AutoReplenishment = true
                    });
                });

                options.AddPolicy("DefaultServicePolicy", context =>
                {
                    var userId =
                        context.User?.Identity?.Name ??
                        context.Request.Headers["X-Api-Key"].FirstOrDefault() ??
                        "anonymous";

                    var path = context.Request.Path.Value ?? "unknown";
                    var method = context.Request.Method;

                    var key = $"{userId}|{method}:{path}"; // like User123|GET:/products → 5 req/min


                    return RateLimitPartition.GetTokenBucketLimiter(
                        key,
                        _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 5,
                            TokensPerPeriod = 5,
                            ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                            AutoReplenishment = true
                        });

                });
                options.RejectionStatusCode = 429;

            });

            builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


                var app = builder.Build();
                app.UseRateLimiter();
                app.MapReverseProxy();

                app.MapGet("/", () => "Hello World!");

                app.Run();
            
    
        }
    } 
}
