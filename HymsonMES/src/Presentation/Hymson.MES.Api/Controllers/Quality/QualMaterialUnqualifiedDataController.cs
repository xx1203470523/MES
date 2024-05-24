using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（车间物料不良记录）
    /// @author zhaoqing
    /// @date 2024-05-15 11:53:12
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualMaterialUnqualifiedDataController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualMaterialUnqualifiedDataController> _logger;
        /// <summary>
        /// 服务接口（车间物料不良记录）
        /// </summary>
        private readonly IQualMaterialUnqualifiedDataService _qualMaterialUnqualifiedDataService;


        /// <summary>
        /// 构造函数（车间物料不良记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualMaterialUnqualifiedDataService"></param>
        public QualMaterialUnqualifiedDataController(ILogger<QualMaterialUnqualifiedDataController> logger, IQualMaterialUnqualifiedDataService qualMaterialUnqualifiedDataService)
        {
            _logger = logger;
            _qualMaterialUnqualifiedDataService = qualMaterialUnqualifiedDataService;
        }

        /// <summary>
        /// 添加（车间物料不良记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            await _qualMaterialUnqualifiedDataService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（车间物料不良记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualMaterialUnqualifiedDataSaveDto saveDto)
        {
            await _qualMaterialUnqualifiedDataService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 处置（车间物料不良记录）
        /// </summary>
        /// <param name="disposalDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("disposal")]
        public async Task DisposalAsync([FromBody] QualMaterialUnqualifiedDataDisposalDto disposalDto)
        {
            await _qualMaterialUnqualifiedDataService.DisposalAsync(disposalDto);
        }

        /// <summary>
        /// 删除（车间物料不良记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualMaterialUnqualifiedDataService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（车间物料不良记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualMaterialUnqualifiedDataDto?> QueryByIdAsync(long id)
        {
            return await _qualMaterialUnqualifiedDataService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（车间物料不良记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualMaterialUnqualifiedDataViewDto>> QueryPagedListAsync([FromQuery] QualMaterialUnqualifiedDataPagedQueryDto pagedQueryDto)
        {
            return await _qualMaterialUnqualifiedDataService.GetPagedListAsync(pagedQueryDto);
        }

    }
}