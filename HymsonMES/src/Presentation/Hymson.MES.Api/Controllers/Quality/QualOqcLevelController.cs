using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（OQC检验水平）
    /// @author Czhipu
    /// @date 2024-02-02 02:04:09
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualOqcLevelController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualOqcLevelController> _logger;

        /// <summary>
        /// 服务接口（OQC检验水平）
        /// </summary>
        private readonly IQualOqcLevelService _qualOqcLevelService;

        /// <summary>
        /// 构造函数（OQC检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualOqcLevelService"></param>
        public QualOqcLevelController(ILogger<QualOqcLevelController> logger, IQualOqcLevelService qualOqcLevelService)
        {
            _logger = logger;
            _qualOqcLevelService = qualOqcLevelService;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("OQC检验水平", BusinessType.INSERT)]
        [PermissionDescription("quality:qualOQCLevel:insert")]
        public async Task AddAsync([FromBody] QualOqcLevelSaveDto saveDto)
        {
            await _qualOqcLevelService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("OQC检验水平", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualOQCLevel:update")]
        public async Task UpdateAsync([FromBody] QualOqcLevelSaveDto saveDto)
        {
            await _qualOqcLevelService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("OQC检验水平", BusinessType.DELETE)]
        [PermissionDescription("quality:qualOQCLevel:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualOqcLevelService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualOqcLevelDto>> GetPagedListAsync([FromQuery] QualOqcLevelPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcLevelService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualOqcLevelDto?> QueryByIdAsync(long id)
        {
            return await _qualOqcLevelService.QueryByIdAsync(id);
        }


    }
}