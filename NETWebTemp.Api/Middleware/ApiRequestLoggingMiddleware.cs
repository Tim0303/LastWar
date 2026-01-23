using NLog;
using System.Text;

namespace NETWebTemp.Api.Middleware
{
    public class ApiRequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiRequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
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

            // 建立 NLog LogEventInfo 並設定 properties
            var logEvent = new LogEventInfo(NLog.LogLevel.Info, "database", "API Request");
            logEvent.Properties["logtime"] = DateTime.Now;
            logEvent.Properties["logger"] = "ApiRequestLoggingMiddleware";
            logEvent.Properties["loglevel"] = "Info";
            logEvent.Properties["message"] = "API Request";
            logEvent.Properties["httpmethod"] = context.Request.Method;
            logEvent.Properties["path"] = context.Request.Path + context.Request.QueryString;
            logEvent.Properties["parameters"] = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : "";
            logEvent.Properties["requestbody"] = requestBody;
            logEvent.Properties["requestform"] = requestForm;
            logEvent.Properties["httpstatuscode"] = statusCode;
            logEvent.Properties["ip"] = context.Connection.RemoteIpAddress?.ToString();
            logEvent.Properties["userid"] = userId;

            var logger = LogManager.GetLogger("database");
            logger.Log(logEvent);
        }
    }
}