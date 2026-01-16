using EFPowerTools.Models;
using Microsoft.EntityFrameworkCore;

namespace NETWebTemp.Api.Settings
{
    /// <summary>
    /// 初始化 Function 設定
    /// </summary>
    public static class InitSqlFunctionSettings
    {
        /// <summary>
        /// 初始化 Function 設定
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void AddInitSqlFunctionSettings(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDBContext>();

                AddGetUpdaterInfo(dbContext);

                AddGetCadastralFullNo(dbContext);
            }
        }

        /// <summary>
        /// fn_GetUpdaterInfo
        /// </summary>
        /// <param name="dbContext"></param>
        public static void AddGetUpdaterInfo(MyDBContext dbContext)
        {
            // 刪除 function（如果存在）
            dbContext.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'dbo.fn_GetUpdaterInfo', N'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetUpdaterInfo;
");

            // 建立 function
            dbContext.Database.ExecuteSqlRaw(@"
CREATE FUNCTION [dbo].[fn_GetUpdaterInfo]
                (
                    @userId uniqueidentifier
                )
                RETURNS NVARCHAR(250)
                AS
                BEGIN
                    DECLARE @UpdaterInfo NVARCHAR(250) = N'';

                    SELECT @UpdaterInfo = ISNULL(TRIM(CONCAT (
                                ISNULL(Unit.UnitName, ''),
                                CASE WHEN Unit.UnitName IS NOT NULL THEN ' ' ELSE '' END,
                                ISNULL(ApplicationUser.Name, '')
                                )), N'')
                    FROM ApplicationUser
                    LEFT JOIN Unit ON ApplicationUser.UnitId = Unit.UnitId
                    WHERE ApplicationUser.Id = @UserId;

                    RETURN @UpdaterInfo;
                END
");
        }

        /// <summary>
        /// fn_GetCadastralFullNo
        /// </summary>
        /// <param name="dbContext"></param>
        public static void AddGetCadastralFullNo(MyDBContext dbContext)
        {
            // 刪除 function（如果存在）
            dbContext.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'dbo.fn_GetCadastralFullNo', N'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_GetCadastralFullNo;
");

            // 建立 function
            dbContext.Database.ExecuteSqlRaw(@"
                CREATE FUNCTION [dbo].[fn_GetCadastralFullNo]
                (
                    @CadastralId uniqueidentifier
                )
                RETURNS NVARCHAR(250)
                AS
                BEGIN
                    DECLARE @CadastralFullNo NVARCHAR(250) = N'';

                    SELECT @CadastralFullNo = ISNULL(TRIM(CONCAT (
                                city.Name,
                                town.Name,
                                section.Name,
                                ' ',
                                LandMainNo,
                                '-',
                                LandSubNo
                                )), N'')
                    FROM Cadastral c
                    JOIN CityCode city on c.CityCodeId = city.CityCodeId
                    JOIN TownCode town on c.TownCodeId = town.TownCodeId
                    JOIN SectionCode section on c.SectionCodeId = section.SectionCodeId
                    WHERE CadastralId = @CadastralId;

                    RETURN @CadastralFullNo;
                END
");
        }
    }
}