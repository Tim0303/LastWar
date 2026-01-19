using Microsoft.AspNetCore.Mvc;
using NETWebTemp.Service.Interfaces;
using NETWebTemp.Service.Parameters.Player;
using NETWebTemp.Service.ViewModels.Player;

namespace NETWebTemp.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        /// <summary>
        /// 取得玩家戰力清單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<List<PlayerListVm>>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> List([FromQuery] PlayerListParam param)
        {
            var result = await _service.List(param);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("讀取失敗");
            }
        }

        /// <summary>
        /// 取得新玩家資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<PlayerDetailVm>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> GetDetail([FromQuery] string playerId)
        {
            var result = await _service.GetDetail(playerId);
            return Ok(result);
        }

        /// <summary>
        /// 建立新玩家資訊
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<object>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> Create([FromBody] PlayerUpdateVm param)
        {
            var result = await _service.Create(param);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("建立失敗");
            }
        }

        /// <summary>
        /// 更新玩家戰力
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<object>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> Update([FromBody] PlayerUpdateVm param)
        {
            var result = await _service.Update(param);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("更新失敗");
            }
        }
    }
}