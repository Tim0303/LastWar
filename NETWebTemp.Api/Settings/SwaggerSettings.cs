using Microsoft.OpenApi.Models;

namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// Swagger 設定
    /// </summary>
    public static class SwaggerSettings
    {
        /// <summary>
        /// 加入 Swagger 設定
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSwaggerSettings(this IHostApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "NETWebTemp", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                //加入所有專案的.xml
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name?.StartsWith("NETWebTemp") ?? false);

                foreach (var assembly in assemblies)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");

                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                }

                //顯示Enum的Description
                // options.DocumentFilter<SwaggerEnumDocumentFilter>();
                //根據有沒有AllowAnonymous來顯示401標籤
                // options.OperationFilter<UnauthorizedResponseFilter>();
            });
        }
    }
}