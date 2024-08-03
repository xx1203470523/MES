using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushSwitch;
using Hymson.MES.Services.Services.NioPushSwitch;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.NioPushSwitch
{
    /// <summary>
    /// 控制器（蔚来推送开关）
    /// @author Yxx
    /// @date 2024-08-02 05:12:26
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NioPushSwitchController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<NioPushSwitchController> _logger;
        /// <summary>
        /// 服务接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchService _nioPushSwitchService;


        /// <summary>
        /// 构造函数（蔚来推送开关）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushSwitchService"></param>
        public NioPushSwitchController(ILogger<NioPushSwitchController> logger, INioPushSwitchService nioPushSwitchService)
        {
            _logger = logger;
            _nioPushSwitchService = nioPushSwitchService;
        }

        /// <summary>
        /// 添加（蔚来推送开关）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("nio:niopushswitch:insert")]
        public async Task AddAsync([FromBody] NioPushSwitchSaveDto saveDto)
        {
             await _nioPushSwitchService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（蔚来推送开关）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("nio:niopushswitch:update")]
        public async Task UpdateAsync([FromBody] NioPushSwitchSaveDto saveDto)
        {
             await _nioPushSwitchService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 更新状态（蔚来推送开关）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [PermissionDescription("nio:niopushswitch:update")]
        public async Task UpdateStatusEnableAsync([FromBody] NioPushSwitchModifyDto saveDto)
        {
            await _nioPushSwitchService.ModifyEnableAsync(saveDto);
        }

        /// <summary>
        /// 删除（蔚来推送开关）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("nio:niopushswitch:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _nioPushSwitchService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（蔚来推送开关）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NioPushSwitchDto?> QueryByIdAsync(long id)
        {
            return await _nioPushSwitchService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（蔚来推送开关）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<NioPushSwitchDto>> QueryPagedListAsync([FromQuery] NioPushSwitchPagedQueryDto pagedQueryDto)
        {
            return await _nioPushSwitchService.GetPagedListAsync(pagedQueryDto);
        }

    }
}