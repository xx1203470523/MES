using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（操作面板按钮）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFacePlateButtonController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<ManuFacePlateButtonController> _logger;

        /// <summary>
        /// 接口（操作面板按钮）
        /// </summary>
        private readonly IManuFacePlateButtonService _manuFacePlateButtonService;


        /// <summary>
        /// 构造函数（操作面板按钮）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFacePlateButtonService"></param>
        public ManuFacePlateButtonController(ILogger<ManuFacePlateButtonController> logger,
            IManuFacePlateButtonService manuFacePlateButtonService)
        {
            _logger = logger;
            _manuFacePlateButtonService = manuFacePlateButtonService;
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
        [LogDescription("操作面板按钮", BusinessType.INSERT)]
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
        [LogDescription("操作面板按钮", BusinessType.UPDATE)]
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
        [LogDescription("操作面板按钮", BusinessType.DELETE)]
        public async Task DeleteManuFacePlateButtonAsync([FromBody] long[] ids)
        {
            await _manuFacePlateButtonService.DeletesManuFacePlateButtonAsync(ids);
        }


        /// <summary>
        /// 按钮（回车）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("enter")]
        [LogDescription("回车", BusinessType.OTHER)]
        [AllowAnonymous]
        public async Task<Dictionary<string, JobResponseDto>> EnterAsync(EnterRequestDto dto)
        {
            return await _manuFacePlateButtonService.EnterAsync(dto);
        }

        /// <summary>
        /// 按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("button")]
        [LogDescription("点击", BusinessType.OTHER)]
        [AllowAnonymous]
        public async Task<Dictionary<string, JobResponseDto>> ClickAsync(ButtonRequestDto dto)
        {
            return await _manuFacePlateButtonService.ClickAsync(dto);
        }

        /// <summary>
        /// 参数收集（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("parameterCollect")]
        [LogDescription("点击", BusinessType.OTHER)]
        [AllowAnonymous]
        public async Task<int> ParameterCollectAsync(ProductProcessParameterDto dto)
        {
            return await _manuFacePlateButtonService.ProductParameterCollectAsync(dto);
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpPut("test")]
        [LogDescription("测试", BusinessType.OTHER)]
        [AllowAnonymous]
        public async Task TeastAsync()
        {
            await Task.CompletedTask;
        }

    }
}