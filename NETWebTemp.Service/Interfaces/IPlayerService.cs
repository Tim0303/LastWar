using NETWebTemp.Service.Parameters.Player;
using NETWebTemp.Service.ViewModels.Player;

namespace NETWebTemp.Service.Interfaces
{
    public interface IPlayerService
    {
        /// <summary>
        /// 取得玩家戰力清單
        /// </summary>
        /// <returns></returns>
        Task<List<PlayerListVm>> List(PlayerListParam param);

        /// <summary>
        /// 建立新玩家資訊
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> Create(PlayerUpdateVm param);

        /// <summary>
        /// 更新玩家戰力
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> Update(PlayerUpdateVm param);

        /// <summary>
        /// 取得新玩家資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlayerDetailVm> GetDetail(string playerId);
    }
}