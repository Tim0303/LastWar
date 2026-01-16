using NETWebTemp.Service.Interfaces;
using NETWebTemp.Service.Services;

namespace NETWebTemp.Api.Settings
{
    public static class DISettings
    {
        public static void AddDISettings(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
        }
    }
}
