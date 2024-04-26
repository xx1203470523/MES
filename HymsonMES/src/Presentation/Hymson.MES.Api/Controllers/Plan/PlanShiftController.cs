using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（班制）
    /// @author Jam
    /// @date 2024-01-24 11:57:04
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanShiftController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<PlanShiftController> _logger;
        /// <summary>
        /// 服务接口（班制）
        /// </summary>
        private readonly IPlanShiftService _planShiftService;


        /// <summary>
        /// 构造函数（班制）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planShiftService"></param>
        public PlanShiftController(ILogger<PlanShiftController> logger, IPlanShiftService planShiftService)
        {
            _logger = logger;
            _planShiftService = planShiftService;
        }

        /// <summary>
        /// 添加（班制）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("班制", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] PlanShiftSaveDto saveDto)
        {
            await _planShiftService.ModifyAsync(saveDto, InteShiftModifyTypeEnum.create);
        }

        /// <summary>
        /// 更新（班制）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("班制", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] PlanShiftSaveDto saveDto)
        {
            await _planShiftService.ModifyAsync(saveDto, InteShiftModifyTypeEnum.modify);
        }

        /// <summary>
        /// 删除（班制）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("班制", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _planShiftService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（班制）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<PlanShiftDto?> QueryByIdAsync(long id)
        {
            return await _planShiftService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（班制）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<PlanShiftDto>> QueryPagedListAsync([FromQuery] PlanShiftPagedQueryDto pagedQueryDto)
        {
            return await _planShiftService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IEnumerable<PlanShiftDto>> QueryAllAsync()
        {
            return await _planShiftService.GetAllAsync();
        }
        
        /// <summary>
        /// 获取详情ID
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        [HttpGet("getShiftDetailbyId/{mainId}")]
        public async Task<IEnumerable<PlanShiftDetailDto>> GetByMainIdAsync(long mainId)
        {
            return await _planShiftService.GetByMainIdAsync(mainId);
        }


        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatus")]
        [LogDescription("状态变更", BusinessType.UPDATE)]
        public async Task UpdateStatus([FromBody] ChangeStatusDto dto)
        {
            await _planShiftService.UpdateStatusAsync(new ChangeStatusDto { Id = dto.Id, Status = dto.Status });
        }

    }
}