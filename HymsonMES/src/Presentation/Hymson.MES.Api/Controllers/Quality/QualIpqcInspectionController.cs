using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（IPQC检验项目）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIpqcInspectionController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIpqcInspectionController> _logger;
        /// <summary>
        /// 服务接口（IPQC检验项目）
        /// </summary>
        private readonly IQualIpqcInspectionService _qualIpqcInspectionService;


        /// <summary>
        /// 构造函数（IPQC检验项目）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIpqcInspectionService"></param>
        public QualIpqcInspectionController(ILogger<QualIpqcInspectionController> logger, IQualIpqcInspectionService qualIpqcInspectionService)
        {
            _logger = logger;
            _qualIpqcInspectionService = qualIpqcInspectionService;
        }

        /// <summary>
        /// 添加（IPQC检验项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("IPQC检验项目", BusinessType.INSERT)]
        [PermissionDescription("quality:ipqcInspection:insert")]
        public async Task AddAsync([FromBody] QualIpqcInspectionSaveDto saveDto)
        {
             await _qualIpqcInspectionService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（IPQC检验项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("IPQC检验项目", BusinessType.UPDATE)]
        [PermissionDescription("quality:ipqcInspection:update")]
        public async Task UpdateAsync([FromBody] QualIpqcInspectionSaveDto saveDto)
        {
             await _qualIpqcInspectionService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（IPQC检验项目）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("IPQC检验项目", BusinessType.DELETE)]
        [PermissionDescription("quality:ipqcInspection:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（IPQC检验项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIpqcInspectionDto?> QueryByIdAsync(long id)
        {
            return await _qualIpqcInspectionService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（IPQC检验项目）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIpqcInspectionViewDto>> QueryPagedListAsync([FromQuery] QualIpqcInspectionPagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 根据ID获取关联明细列表（IPQC检验项目参数）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<QualIpqcInspectionParameterDto>?> QueryDetailsByMainIdAsync(long id)
        {
            return await _qualIpqcInspectionService.QueryDetailsByMainIdAsync(id);
        }

        /// <summary>
        /// 根据ID获取检验规则列表（IPQC检验项目检验规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("rules/{id}")]
        public async Task<IEnumerable<QualIpqcInspectionRuleDto>?> QueryRulesByMainIdAsync(long id)
        {
            return await _qualIpqcInspectionService.QueryRulesByMainIdAsync(id);
        }

        #region 状态变更
        /// <summary>
        /// 启用（IPQC检验项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("IPQC检验项目", BusinessType.UPDATE)]
        [PermissionDescription("quality:ipqcInspection:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _qualIpqcInspectionService.UpdateStatusEnable(id);
        }

        /// <summary>
        /// 保留（IPQC检验项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("IPQC检验项目", BusinessType.UPDATE)]
        [PermissionDescription("quality:ipqcInspection:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _qualIpqcInspectionService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（IPQC检验项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("IPQC检验项目", BusinessType.UPDATE)]
        [PermissionDescription("quality:ipqcInspection:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _qualIpqcInspectionService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}