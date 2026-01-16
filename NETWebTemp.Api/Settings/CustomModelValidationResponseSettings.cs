using Microsoft.AspNetCore.Mvc;

namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// 自訂Model驗證失敗訊息設定
    /// </summary>
    public static class CustomModelValidationResponseSettings
    {
        /// <summary>
        /// 自訂Model驗證失敗訊息
        /// </summary>
        /// <param name="builder"></param>
        public static void AddCustomModelValidationResponse(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .ToDictionary(
                            model => char.ToLowerInvariant(model.Key[0]) + model.Key.Substring(1),
                            model => string.Join(",", model.Value?.Errors.Select(e => e.ErrorMessage) ?? [])
                        );

                    return new BadRequestObjectResult(errors);
                };
            });
        }
    }
}