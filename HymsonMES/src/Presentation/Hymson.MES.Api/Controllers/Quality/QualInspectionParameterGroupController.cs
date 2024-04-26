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
    /// 控制器（全检参数表）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualInspectionParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualInspectionParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（全检参数表）
        /// </summary>
        private readonly IQualInspectionParameterGroupService _qualInspectionParameterGroupService;


        /// <summary>
        /// 构造函数（全检参数表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualInspectionParameterGroupService"></param>
        public QualInspectionParameterGroupController(ILogger<QualInspectionParameterGroupController> logger, IQualInspectionParameterGroupService qualInspectionParameterGroupService)
        {
            _logger = logger;
            _qualInspectionParameterGroupService = qualInspectionParameterGroupService;
        }

        /// <summary>
        /// 添加（全检参数表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("全检参数表", BusinessType.INSERT)]
        [PermissionDescription("quality:qualInspectionParameterGroup:insert")]
        public async Task<long> AddAsync([FromBody] QualInspectionParameterGroupSaveDto saveDto)
        {
            return await _qualInspectionParameterGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（全检参数表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("全检参数表", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualInspectionParameterGroup:update")]
        public async Task UpdateAsync([FromBody] QualInspectionParameterGroupSaveDto saveDto)
        {
            await _qualInspectionParameterGroupService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（全检参数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("全检参数表", BusinessType.DELETE)]
        [PermissionDescription("quality:qualInspectionParameterGroup:delete")]
        public async Task DeletesAsync([FromBody] long[] ids)
        {
            await _qualInspectionParameterGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（全检参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualInspectionParameterGroupInfoDto?> QueryByIdAsync(long id)
        {
            return await _qualInspectionParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 根据ID获取关联明细列表（环境检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<QualInspectionParameterGroupDetailDto>?> QueryDetailsByMainIdAsync(long id)
        {
            return await _qualInspectionParameterGroupService.QueryDetailsByMainIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（全检参数表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualInspectionParameterGroupDto>> QueryPagedListAsync([FromQuery] QualInspectionParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _qualInspectionParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取关联明细列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("details/list")]
        public async Task<PagedInfo<QualInspectionParameterGroupDetailViewDto>> QueryDetailPagedListAsync([FromQuery] QualInspectionParameterGroupDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualInspectionParameterGroupService.QueryDetailPagedListAsync(pagedQueryDto);
        }

        #region 状态变更
        /// <summary>
        /// 启用（全检参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("全检参数表", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualInspectionParameterGroup:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _qualInspectionParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（全检参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("全检参数表", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualInspectionParameterGroup:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _qualInspectionParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（全检参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("全检参数表", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualInspectionParameterGroup:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _qualInspectionParameterGroupService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}