using System.Text;

namespace MyMiddleware.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var logData = new StringBuilder();

            logData.AppendLine($"Schema: {request.Scheme}");
            logData.AppendLine($"Host: {request.Host}");
            logData.AppendLine($"Path: {request.Path}");
            logData.AppendLine($"Query String: {request.QueryString}");

            request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                logData.AppendLine($"Request Body: {body}");
                request.Body.Position = 0;
            }

            await File.AppendAllTextAsync("logs.txt", logData.ToString() + Environment.NewLine);

            await _next(context);
        }
    }
}