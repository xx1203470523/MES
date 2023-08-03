using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（环境检验参数表）
    /// @author Czhipu
    /// @date 2023-07-22 10:54:48
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualEnvParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualEnvParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（环境检验参数表）
        /// </summary>
        private readonly IQualEnvParameterGroupService _qualEnvParameterGroupService;


        /// <summary>
        /// 构造函数（环境检验参数表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualEnvParameterGroupService"></param>
        public QualEnvParameterGroupController(ILogger<QualEnvParameterGroupController> logger, IQualEnvParameterGroupService qualEnvParameterGroupService)
        {
            _logger = logger;
            _qualEnvParameterGroupService = qualEnvParameterGroupService;
        }

        /// <summary>
        /// 添加（环境检验参数表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("quality:qualEnvParameterGroup:insert")]
        public async Task AddAsync([FromBody] QualEnvParameterGroupSaveDto saveDto)
        {
            await _qualEnvParameterGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（环境检验参数表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("quality:qualEnvParameterGroup:update")]
        public async Task UpdateAsync([FromBody] QualEnvParameterGroupSaveDto saveDto)
        {
            await _qualEnvParameterGroupService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（环境检验参数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("quality:qualEnvParameterGroup:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualEnvParameterGroupService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（环境检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualEnvParameterGroupInfoDto?> QueryByIdAsync(long id)
        {
            return await _qualEnvParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 根据ID获取关联明细列表（环境检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<QualEnvParameterGroupDetailDto>?> QueryDetailsByMainIdAsync(long id)
        {
            return await _qualEnvParameterGroupService.QueryDetailsByMainIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（环境检验参数表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualEnvParameterGroupDto>> QueryPagedListAsync([FromQuery] QualEnvParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _qualEnvParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }

    }
}