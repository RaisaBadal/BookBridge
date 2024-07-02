namespace BookBridge.API.CustomMiddlwares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<LoggingMiddleware> log;

        public LoggingMiddleware(RequestDelegate request, ILogger<LoggingMiddleware> log)
        {
            this.request = request;
            this.log = log;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            log.LogInformation($"Handling request: {context.Request.Method} {context.Request.Path}");
            await request(context);
            log.LogInformation($"Finished handling request: {context.Response.StatusCode}");
        }
    }
}