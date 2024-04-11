using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（FQC检验参数组明细）
    /// @author Jam
    /// @date 2024-03-12 07:32:32
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualFqcParameterGroupDetailController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualFqcParameterGroupDetailController> _logger;
        /// <summary>
        /// 服务接口（FQC检验参数组明细）
        /// </summary>
        private readonly IQualFqcParameterGroupDetailService _qualFqcParameterGroupDetailService;


        /// <summary>
        /// 构造函数（FQC检验参数组明细）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualFqcParameterGroupDetailService"></param>
        public QualFqcParameterGroupDetailController(ILogger<QualFqcParameterGroupDetailController> logger, IQualFqcParameterGroupDetailService qualFqcParameterGroupDetailService)
        {
            _logger = logger;
            _qualFqcParameterGroupDetailService = qualFqcParameterGroupDetailService;
        }

        /// <summary>
        /// 添加（FQC检验参数组明细）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualFqcParameterGroupDetailSaveDto saveDto)
        {
             await _qualFqcParameterGroupDetailService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（FQC检验参数组明细）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualFqcParameterGroupDetailSaveDto saveDto)
        {
             await _qualFqcParameterGroupDetailService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（FQC检验参数组明细）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualFqcParameterGroupDetailService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（FQC检验参数组明细）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualFqcParameterGroupDetailDto?> QueryByIdAsync(long id)
        {
            return await _qualFqcParameterGroupDetailService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（FQC检验参数组明细）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualFqcParameterGroupDetailDto>> QueryPagedListAsync([FromQuery] QualFqcParameterGroupDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualFqcParameterGroupDetailService.GetPagedListAsync(pagedQueryDto);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<QualFqcParameterGroupDetailOutputDto>> GetListAsync([FromQuery] QualFqcParameterGroupDetailQueryDto query)
        {
            return await _qualFqcParameterGroupDetailService.GetListAsync(query);
        }

    }
}