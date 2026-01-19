using System.Text;

namespace NETWebTemp.Api.Middleware
{
    public class ApiRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiRequestLoggingMiddleware> _logger;

        public ApiRequestLoggingMiddleware(RequestDelegate next, ILogger<ApiRequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // 取得 Request Body
            context.Request.EnableBuffering();
            string requestBody = "";
            if (context.Request.ContentLength > 0)
            {
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            // 取得 Request Form
            string requestForm = "";
            if (context.Request.HasFormContentType)
            {
                foreach (var kv in context.Request.Form)
                {
                    requestForm += $"{kv.Key}={kv.Value}&";
                }
                requestForm = requestForm.TrimEnd('&');
            }

            await _next(context);

            // 取得 HttpStatusCode
            var statusCode = context.Response.StatusCode;

            // 取得 UserId (依你的驗證方式調整)
            string userId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.FindFirst("sub")?.Value ?? context.User.Identity.Name
                : null;

            // 記錄 log
            _logger.LogInformation(
                "API Request | Method: {Method} | Path: {Path} | Query: {Query} | Body: {Body} | Form: {Form} | Status: {Status} | IP: {IP} | UserId: {UserId}",
                context.Request.Method,
                context.Request.Path + context.Request.QueryString,
                context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "",
                requestBody,
                requestForm,
                statusCode,
                context.Connection.RemoteIpAddress?.ToString(),
                userId
            );
        }
    }
}