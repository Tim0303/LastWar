using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NETWebTemp.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using NETWebTemp.Service.ViewModels.Player;
using EFPowerTools.Models;
using Microsoft.EntityFrameworkCore;
using EFPowerTools.Models.dbo;
using NETWebTemp.Service.Parameters.Player;
using NETWebTemp.Common.Extensions;

namespace NETWebTemp.Service.Services
{
    /// <summary>
    ///
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions;
        private readonly TimeProvider _timeProvider;
        private readonly MyDBContext _dbContext;

        /// <summary>
        /// 建構式
        /// </summary>
        public PlayerService(
            IConfiguration configuration,
            IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
            TimeProvider timeProvider,
            MyDBContext dbContext
            )
        {
            _configuration = configuration;
            _bearerTokenOptions = bearerTokenOptions;
            _timeProvider = timeProvider;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 取得玩家戰力清單
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PlayerListVm>> List(PlayerListParam param)
        {
            var data = await _dbContext.Players
                .WhereIf(!string.IsNullOrEmpty(param.Name), x => x.Name.Contains(param.Name))
                .Select(s => new PlayerListVm()
                {
                    PlayerId = s.PlayerId,
                    Name = s.Name,
                    HeadquartersLv = s.HeadquartersLv,
                    TankPower = s.TankPower,
                    AircraftPower = s.AircraftPower,
                    MissilePower = s.MissilePower,
                    HasT10 = s.HasT10,
                    TechSpecialForces = s.TechSpecialForces,
                    TechSiegeToSeize = s.TechSiegeToSeize,
                    TechHero = s.TechHero,
                    TechDefenseFortifications = s.TechDefenseFortifications,
                    UpdateTime = s.UpdateTime,
                })
                .OrderByDescending(x => x.HeadquartersLv)
                .ToListAsync();

            return data;
        }

        /// <summary>
        /// 建立新玩家資訊
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Create(PlayerUpdateVm param)
        {
            var data = await _dbContext.Players
                .Where(f => f.PlayerId == param.PlayerId)
                .FirstOrDefaultAsync();

            if (data != null)
            {
                return false;
            }
            else
            {
                data = new Player()
                {
                    PlayerId = param.PlayerId,
                    Name = param.Name,
                    HeadquartersLv = param.HeadquartersLv,
                    TankPower = param.TankPower,
                    AircraftPower = param.AircraftPower,
                    MissilePower = param.MissilePower,
                    HasT10 = param.HasT10,
                    TechSpecialForces = param.TechSpecialForces,
                    TechSiegeToSeize = param.TechSiegeToSeize,
                    TechHero = param.TechHero,
                    TechDefenseFortifications = param.TechDefenseFortifications,
                };

                await _dbContext.Players.AddAsync(data);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新玩家戰力
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> Update(PlayerUpdateVm param)
        {
            var data = await _dbContext.Players
                .Where(f => f.PlayerId == param.PlayerId)
                .FirstOrDefaultAsync();

            if (data != null)
            {
                // 建立快照
                var snapshot = new PlayerSnapshot()
                {
                    PlayerId = data.PlayerId,
                    Name = data.Name,
                    HeadquartersLv = data.HeadquartersLv,
                    TankPower = data.TankPower,
                    AircraftPower = data.AircraftPower,
                    MissilePower = data.MissilePower,
                    HasT10 = data.HasT10,
                    TechSpecialForces = data.TechSpecialForces,
                    TechSiegeToSeize = data.TechSiegeToSeize,
                    TechHero = data.TechHero,
                    TechDefenseFortifications = data.TechDefenseFortifications,
                };

                await _dbContext.PlayerSnapshots.AddAsync(snapshot);

                data.HeadquartersLv = param.HeadquartersLv;
                data.TankPower = param.TankPower;
                data.AircraftPower = param.AircraftPower;
                data.MissilePower = param.MissilePower;
                data.HasT10 = param.HasT10;
                data.TechSpecialForces = param.TechSpecialForces;
                data.TechSiegeToSeize = param.TechSiegeToSeize;
                data.TechHero = param.TechHero;
                data.TechDefenseFortifications = param.TechDefenseFortifications;
            }
            else
            {
                return false;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取得新玩家資訊
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PlayerDetailVm> GetDetail(string playerId)
        {
            var data = await _dbContext.Players
                .Where(f => f.PlayerId == playerId)
                .Select(s => new PlayerDetailVm()
                {
                    PlayerId = s.PlayerId,
                    Name = s.Name,
                    HeadquartersLv = s.HeadquartersLv,
                    TankPower = s.TankPower,
                    AircraftPower = s.AircraftPower,
                    MissilePower = s.MissilePower,
                    HasT10 = s.HasT10,
                    TechSpecialForces = s.TechSpecialForces,
                    TechSiegeToSeize = s.TechSiegeToSeize,
                    TechHero = s.TechHero,
                    TechDefenseFortifications = s.TechDefenseFortifications,
                })
                .FirstOrDefaultAsync();

            return data;
        }
    }
}