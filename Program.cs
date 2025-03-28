using MyMiddleware.Middleware;

namespace MyMiddleware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<LoggingMiddleware>();

            app.MapPost("/test", async (HttpContext context) =>
            {
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                return Results.Ok($"Received: {requestBody}");
            });

            app.Run();
        }
    }
}