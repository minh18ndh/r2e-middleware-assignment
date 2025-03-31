using MyMiddleware.Middleware;

namespace MyMiddleware
{
    public class Startup
    {
        public void Configure(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<LoggingMiddleware>();

            app.MapPost("/test", async (HttpContext context) =>
            {
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                return Results.Ok($"Received: {requestBody}");
            });
        }
    }
}