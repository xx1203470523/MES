using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（操作面板按钮）
    /// @author Karl
    /// @date 2023-04-01 02:58:19
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFacePlateButtonController : ControllerBase
    {
        /// <summary>
        /// 接口（操作面板按钮）
        /// </summary>
        private readonly IManuFacePlateButtonService _manuFacePlateButtonService;
        private readonly ILogger<ManuFacePlateButtonController> _logger;

        /// <summary>
        /// 构造函数（操作面板按钮）
        /// </summary>
        /// <param name="manuFacePlateButtonService"></param>
        /// <param name="logger"></param>
        public ManuFacePlateButtonController(IManuFacePlateButtonService manuFacePlateButtonService, ILogger<ManuFacePlateButtonController> logger)
        {
            _manuFacePlateButtonService = manuFacePlateButtonService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（操作面板按钮）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFacePlateButtonDto>> QueryPagedManuFacePlateButtonAsync([FromQuery] ManuFacePlateButtonPagedQueryDto parm)
        {
            return await _manuFacePlateButtonService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（操作面板按钮）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByIdAsync(long id)
        {
            return await _manuFacePlateButtonService.QueryManuFacePlateButtonByIdAsync(id);
        }

        /// <summary>
        ///根据ButtonID查询按钮信息和关联JOB
        /// </summary>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        [HttpGet("queryButtonBybuttonId/{buttonId}")]
        public async Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByButtonIdAsync(long buttonId)
        {
            return await _manuFacePlateButtonService.QueryManuFacePlateButtonByButtonIdAsync(buttonId);
        }

        /// <summary>
        /// 添加（操作面板按钮）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddManuFacePlateButtonAsync([FromBody] ManuFacePlateButtonCreateDto parm)
        {
            await _manuFacePlateButtonService.CreateManuFacePlateButtonAsync(parm);
        }

        /// <summary>
        /// 更新（操作面板按钮）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateManuFacePlateButtonAsync([FromBody] ManuFacePlateButtonModifyDto parm)
        {
            await _manuFacePlateButtonService.ModifyManuFacePlateButtonAsync(parm);
        }

        /// <summary>
        /// 删除（操作面板按钮）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuFacePlateButtonAsync([FromBody] long[] ids)
        {
            await _manuFacePlateButtonService.DeletesManuFacePlateButtonAsync(ids);
        }


        /// <summary>
        /// 按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("button")]
        [AllowAnonymous]
        public async Task<Dictionary<string, JobResponseDto>> ClickAsync(ButtonRequestDto dto)
        {
            return await _manuFacePlateButtonService.ClickAsync(dto);
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("inStation")]
        [AllowAnonymous]
        public async Task<Dictionary<string, JobResponseDto>> InStationAsync(ButtonRequestDto dto)
        {
            return await _manuFacePlateButtonService.InStationAsync(dto);
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("outStation")]
        [AllowAnonymous]
        public async Task<Dictionary<string, JobResponseDto>> OutStationAsync(ButtonRequestDto dto)
        {
            return await _manuFacePlateButtonService.OutStationAsync(dto);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPut("test")]
        [AllowAnonymous]
        public async Task TeastAsync()
        {
            await Task.CompletedTask;
        }

    }
}