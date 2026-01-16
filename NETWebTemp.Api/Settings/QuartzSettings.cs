// using Quartz;

namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// Quartz排程設定
    /// </summary>
    public static class QuartzSettings
    {
        /// <summary>
        /// 加入 Quartz排程設定
        /// </summary>
        /// <param name="builder"></param>
        public static void AddQuartzSettings(this IHostApplicationBuilder builder)
        {
            /**
            // 新增範例排程
            builder.Services.AddQuartz(quartz =>
            {
                // 有額度限制debug不立即執行
#if !DEBUG
                //quartz.ScheduleJob<UpdateCodeJob>(trigger => trigger
                //    .WithIdentity("immediateUpdateCode Trigger")
                //    .StartNow()
                //    .WithDescription("啟用時立即執行一次更新地政司代碼")
                //);
# endif
                quartz.ScheduleJob<UpdateCodeJob>(trigger => trigger
                    .WithIdentity("UpdateCode Trigger")
                    .WithCronSchedule("0 0 0 1 * ?")
                    .StartNow()
                    .WithDescription("每月固定更新地政司代碼")
                );

                if (builder.Configuration.GetValue<bool>("CashFlowFile:IsEnable"))
                {
                    quartz.ScheduleJob<DailyWriteOffJob>(trigger => trigger
                        .WithIdentity("DailyWriteOff Trigger")
                        .WithCronSchedule("0 0 0 * * ?")
                        .StartNow()
                        .WithDescription("每日00:00時抓取銷帳檔"));
                }

                if (builder.Configuration.GetValue<bool>("SmsSettings:IsEnable"))
                {
                    quartz.ScheduleJob<SmsJob>(trigger => trigger
                        .WithIdentity("Sms Trigger")
                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                        .StartNow()
                        .WithDescription("每分鐘執行簡訊排程"));
                }

                quartz.ScheduleJob<UpdateStatusJob>(trigger => trigger
                    .WithIdentity("UpdateStatus Trigger")
                    .WithCronSchedule("0 0 0 * * ?")
                    .StartNow()
                    .WithDescription("每日00:00時更新資料狀態"));

                //quartz.ScheduleJob<CadastralJob>(trigger => trigger
                //    .WithIdentity("Cadastral Trigger")
                //    .WithCronSchedule("0 0 1 * * ?")
                //    .StartNow()
                //    .WithDescription("每日01:00時更新地籍資料"));

                quartz.AddJob<UpdatePaymentStatusesJob>(opts => opts.WithIdentity("UpdatePaymentStatusesJob"));
                quartz.AddTrigger(opts => opts
                    .ForJob("UpdatePaymentStatusesJob")
                    .WithIdentity("10mUpdatePaymentStatusesTrigger")
                    .WithCronSchedule("0 0/10 * * * ?")  // 每10分鐘
                    .WithDescription("每10分鐘更新占用繳款單狀態"));
            });

            builder.Services.AddQuartzHostedService(options =>
            {
                //關閉應用程式前，等待排程執行完畢
                options.WaitForJobsToComplete = true;
            });

            **/
        }
    }
}