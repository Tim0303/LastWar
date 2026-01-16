namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// 檔案大小 設定
    /// </summary>
    public static class FileSizeSettings
    {
        /// <summary>
        /// 加入 檔案大小 設定
        /// </summary>
        /// <param name="builder"></param>
        public static void AddFileSizeSettings(this WebApplicationBuilder builder)
        {
            // 設定 Kestrel 伺服器的最大請求體積
            builder.WebHost.ConfigureKestrel(options =>
            {
                // options.Limits.MaxRequestBodySize = builder.Configuration.GetSection("Attachment").Get<AttachmentConfig>()?.MaxSize ?? 10485760;
            });
        }
    }
}