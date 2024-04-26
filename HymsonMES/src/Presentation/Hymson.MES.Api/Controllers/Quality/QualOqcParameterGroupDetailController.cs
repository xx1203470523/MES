using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（OQC检验参数组明细）
    /// @author Jam
    /// @date 2024-03-12 07:32:32
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualOqcParameterGroupDetailController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualOqcParameterGroupDetailController> _logger;
        /// <summary>
        /// 服务接口（OQC检验参数组明细）
        /// </summary>
        private readonly IQualOqcParameterGroupDetailService _qualOqcParameterGroupDetailService;


        /// <summary>
        /// 构造函数（OQC检验参数组明细）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualOqcParameterGroupDetailService"></param>
        public QualOqcParameterGroupDetailController(ILogger<QualOqcParameterGroupDetailController> logger, IQualOqcParameterGroupDetailService qualOqcParameterGroupDetailService)
        {
            _logger = logger;
            _qualOqcParameterGroupDetailService = qualOqcParameterGroupDetailService;
        }

        /// <summary>
        /// 添加（OQC检验参数组明细）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("OQC检验参数组明细", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] QualOqcParameterGroupDetailSaveDto saveDto)
        {
             await _qualOqcParameterGroupDetailService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（OQC检验参数组明细）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("OQC检验参数组明细", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] QualOqcParameterGroupDetailSaveDto saveDto)
        {
             await _qualOqcParameterGroupDetailService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（OQC检验参数组明细）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("OQC检验参数组明细", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualOqcParameterGroupDetailService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（OQC检验参数组明细）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualOqcParameterGroupDetailDto?> QueryByIdAsync(long id)
        {
            return await _qualOqcParameterGroupDetailService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（OQC检验参数组明细）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualOqcParameterGroupDetailDto>> QueryPagedListAsync([FromQuery] QualOqcParameterGroupDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcParameterGroupDetailService.GetPagedListAsync(pagedQueryDto);
        }

        [HttpGet("list")]
        public async Task<IEnumerable<QualOqcParameterGroupDetailOutputDto>> GetListAsync([FromQuery] QualOqcParameterGroupDetailQueryDto query)
        {
            return await _qualOqcParameterGroupDetailService.GetListAsync(query);
        }

    }
}