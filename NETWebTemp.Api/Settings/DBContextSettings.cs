using EFPowerTools.Models;
using Microsoft.EntityFrameworkCore;

namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// DBContext 設定
    /// </summary>
    public static class DBContextSettings
    {
        /// <summary>
        /// 加入 DBContext 設定
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static string AddDBContextSettings(this IHostApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<MyDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<MyDBContext>(options => options.UseSqlServer(connectionString));

            return connectionString;
        }
    }
}