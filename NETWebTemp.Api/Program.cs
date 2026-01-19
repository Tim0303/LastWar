using NETWebTemp.Api.Middleware;
using NETWebTemp.Api.Settings;
using NLog;
using NLog.Web;

namespace NETWebTemp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 取出configuration實例
            var configuration = builder.Configuration;

            // Add services to the container.

            // 設定DBContext
            var connectionString = builder.AddDBContextSettings();
            // 驗證與授權
            builder.AddIdentitySettings(builder.Configuration);
            // 檔案大小
            builder.AddFileSizeSettings();

            var mvcbuilder = builder.Services.AddControllers();
            // 自訂Model驗證失敗訊息
            mvcbuilder.AddCustomModelValidationResponse();

            // Swagger
            builder.AddSwaggerSettings();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // 註冊DI
            builder.AddDISettings();

            // NLog 設定
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            LogManager.Configuration.Variables["connectionString"] = connectionString;

            var app = builder.Build();

            // Middleware
            app.UseMiddleware<ApiRequestLoggingMiddleware>();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // 初始化 Function 設定
            app.AddInitSqlFunctionSettings();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}