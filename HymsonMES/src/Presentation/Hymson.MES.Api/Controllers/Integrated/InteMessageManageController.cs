using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（消息管理）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteMessageManageController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteMessageManageController> _logger;
        /// <summary>
        /// 服务接口（消息管理）
        /// </summary>
        private readonly IInteMessageManageService _inteMessageManageService;


        /// <summary>
        /// 构造函数（消息管理）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteMessageManageService"></param>
        public InteMessageManageController(ILogger<InteMessageManageController> logger, IInteMessageManageService inteMessageManageService)
        {
            _logger = logger;
            _inteMessageManageService = inteMessageManageService;
        }

        /// <summary>
        /// 触发（消息管理）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("trigger")]
        [LogDescription("消息管理", BusinessType.INSERT)]
        [PermissionDescription("integrated:inteMessageManage:insert")]
        public async Task TriggerAsync([FromBody] InteMessageManageTriggerSaveDto dto)
        {
            await _inteMessageManageService.TriggerAsync(dto);
        }

        /// <summary>
        /// 修改（消息管理）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("消息管理", BusinessType.INSERT)]
        [PermissionDescription("integrated:inteMessageManage:insert")]
        public async Task UpdateAsync([FromBody] InteMessageManageTriggerSaveDto dto)
        {
            await _inteMessageManageService.UpdateAsync(dto);
        }

        /// <summary>
        /// 接收（消息管理）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("receive")]
        [LogDescription("消息管理", BusinessType.UPDATE)]
        [PermissionDescription("integrated:inteMessageManage:update")]
        public async Task ReceiveAsync([FromBody] InteMessageManageReceiveSaveDto dto)
        {
            await _inteMessageManageService.ReceiveAsync(dto);
        }

        /// <summary>
        /// 处理（消息管理）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("handle")]
        [LogDescription("消息管理", BusinessType.UPDATE)]
        [PermissionDescription("integrated:inteMessageManage:update")]
        public async Task HandleAsync([FromBody] InteMessageManageHandleSaveDto dto)
        {
            await _inteMessageManageService.HandleAsync(dto);
        }

        /// <summary>
        /// 关闭（消息管理）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("close")]
        [LogDescription("消息管理", BusinessType.UPDATE)]
        [PermissionDescription("integrated:inteMessageManage:update")]
        public async Task CloseAsync([FromBody] InteMessageManageCloseSaveDto dto)
        {
            await _inteMessageManageService.CloseAsync(dto);
        }

        /// <summary>
        /// 查询详情（消息管理）（触发）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("trigger/{id}")]
        public async Task<InteMessageManageTriggerDto?> QueryTriggerByIdAsync(long id)
        {
            return await _inteMessageManageService.QueryTriggerByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（消息管理）（处理）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("handle/{id}")]
        public async Task<InteMessageManageHandleDto?> QueryHandleByIdAsync(long id)
        {
            return await _inteMessageManageService.QueryHandleByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（消息管理）（关闭）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("close/{id}")]
        public async Task<InteMessageManageCloseDto?> QueryCloseByIdAsync(long id)
        {
            return await _inteMessageManageService.QueryCloseByIdAsync(id);
        }

        /// <summary>
        /// 删除（消息管理）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("消息管理", BusinessType.DELETE)]
        [PermissionDescription("integrated:inteMessageManage:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _inteMessageManageService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（消息管理）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteMessageManageDto>> QueryPagedListAsync([FromQuery] InteMessageManagePagedQueryDto pagedQueryDto)
        {
            return await _inteMessageManageService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取消息编号
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCode")]
        public async Task<string> GetCodeAsync()
        {
            return await _inteMessageManageService.GetCodeAsync();
        }

    }
}