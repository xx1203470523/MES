using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（IQC检验水平）
    /// @author Czhipu
    /// @date 2024-02-02 02:04:09
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIqcLevelController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIqcLevelController> _logger;

        /// <summary>
        /// 服务接口（IQC检验水平）
        /// </summary>
        private readonly IQualIqcLevelService _qualIqcLevelService;

        /// <summary>
        /// 构造函数（IQC检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIqcLevelService"></param>
        public QualIqcLevelController(ILogger<QualIqcLevelController> logger, IQualIqcLevelService qualIqcLevelService)
        {
            _logger = logger;
            _qualIqcLevelService = qualIqcLevelService;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("IQC检验水平", BusinessType.INSERT)]
        [PermissionDescription("quality:qualIQCLevel:insert")]
        public async Task AddAsync([FromBody] QualIqcLevelSaveDto saveDto)
        {
            await _qualIqcLevelService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("IQC检验水平", BusinessType.UPDATE)]
        [PermissionDescription("quality:qualIQCLevel:update")]
        public async Task UpdateAsync([FromBody] QualIqcLevelSaveDto saveDto)
        {
            await _qualIqcLevelService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("IQC检验水平", BusinessType.DELETE)]
        [PermissionDescription("quality:qualIQCLevel:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIqcLevelService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIqcLevelDto>> GetPagedListAsync([FromQuery] QualIqcLevelPagedQueryDto pagedQueryDto)
        {
            return await _qualIqcLevelService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIqcLevelDto?> QueryByIdAsync(long id)
        {
            return await _qualIqcLevelService.QueryByIdAsync(id);
        }


    }
}