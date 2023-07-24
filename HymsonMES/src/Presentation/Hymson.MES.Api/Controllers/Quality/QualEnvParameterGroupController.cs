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
        public async Task AddQualEnvParameterGroupAsync([FromBody] QualEnvParameterGroupSaveDto saveDto)
        {
            await _qualEnvParameterGroupService.CreateQualEnvParameterGroupAsync(saveDto);
        }

        /// <summary>
        /// 更新（环境检验参数表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [PermissionDescription("quality:qualEnvParameterGroup:update")]
        public async Task UpdateQualEnvParameterGroupAsync([FromBody] QualEnvParameterGroupSaveDto saveDto)
        {
            await _qualEnvParameterGroupService.ModifyQualEnvParameterGroupAsync(saveDto);
        }

        /// <summary>
        /// 删除（环境检验参数表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("quality:qualEnvParameterGroup:delete")]
        public async Task DeleteQualEnvParameterGroupAsync([FromBody] long[] ids)
        {
            await _qualEnvParameterGroupService.DeletesQualEnvParameterGroupAsync(ids);
        }

        /// <summary>
        /// 查询详情（环境检验参数表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualEnvParameterGroupInfoDto?> QueryQualEnvParameterGroupByIdAsync(long id)
        {
            return await _qualEnvParameterGroupService.QueryQualEnvParameterGroupByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（环境检验参数表）
        /// </summary>
        /// <param name="parameterVerifyEnvId"></param>
        /// <returns></returns>
        [HttpGet("details/{id}")]
        public async Task<IEnumerable<QualEnvParameterGroupDetailDto>?> QueryDetailsByParameterVerifyEnvIdAsync(long id)
        {
            return await _qualEnvParameterGroupService.QueryDetailsByParameterVerifyEnvIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（环境检验参数表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualEnvParameterGroupDto>> QueryPagedQualEnvParameterGroupAsync([FromQuery] QualEnvParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _qualEnvParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }

    }
}