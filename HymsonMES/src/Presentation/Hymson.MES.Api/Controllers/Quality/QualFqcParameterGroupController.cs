using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
//using Hymson.MES.Services.Dtos.QualityFqcParameterGroup;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;

//using Hymson.MES.Services.Services.QualityFqcParameterGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualityFqcParameterGroup
{
    /// <summary>
    /// 控制器（FQC检验参数组）
    /// @author Jam
    /// @date 2024-03-07 01:47:16
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualFqcParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualFqcParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（FQC检验参数组）
        /// </summary>
        private readonly IQualFqcParameterGroupService _qualFqcParameterGroupService;


        /// <summary>
        /// 构造函数（FQC检验参数组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualFqcParameterGroupService"></param>
        public QualFqcParameterGroupController(ILogger<QualFqcParameterGroupController> logger, IQualFqcParameterGroupService qualFqcParameterGroupService)
        {
            _logger = logger;
            _qualFqcParameterGroupService = qualFqcParameterGroupService;
        }

        /// <summary>
        /// FQC检验项目 ; 单条数据查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpGet("one")]
        [LogDescription("FQC检验项目获取单个", BusinessType.OTHER)]
        public async Task<QualFqcParameterGroupOutputDto> GetOneAsync([FromQuery] QualFqcParameterGroupQueryDto query)
        {
            return await _qualFqcParameterGroupService.GetOneAsync(query);
        }

        /// <summary>
        /// 添加（FQC检验参数组）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("FQC检验项目新增", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] QualFqcParameterGroupDto createDto)
        {
             await _qualFqcParameterGroupService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（FQC检验参数组）
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("FQC检验项目更新", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] QualFqcParameterGroupUpdateDto updateDto)
        {
             await _qualFqcParameterGroupService.ModifyAsync(updateDto);
        }


        /// <summary>
        /// 更新（修改FQC参数组状态）
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateStatus")]
        public async Task ModifStatusAsync([FromBody] UpdateFqcParameterGroupStatusQueryDto updateDto)
        {
            await _qualFqcParameterGroupService.ModifStatusAsync(updateDto);
        }

        /// <summary>
        /// 查询详情（FQC检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualFqcParameterGroupDto?> QueryByIdAsync(long id)
        {
            return await _qualFqcParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（FQC检验参数组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualFqcParameterGroupDto>> QueryPagedListAsync([FromQuery] QualFqcParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _qualFqcParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }


        /// <summary>
        /// 分页查询-包含参数明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [LogDescription("查询FQC分页检验项目", BusinessType.OTHER)]
        public async Task<PagedInfo<QualFqcParameterGroupOutputDto>> GetPagedAsync([FromQuery] QualFqcParameterGroupPagedQueryDto query)
        {
            return await _qualFqcParameterGroupService.GetPagedAsync(query);
        }

        /// <summary>
        /// 删除Fqc检验项目
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [LogDescription("删除FQC检验项目", BusinessType.DELETE)]
        public async Task DeleteSoftAsync(QualFqcParameterGroupDeleteDto deleteDto)
        {
            await _qualFqcParameterGroupService.DeleteAsync(deleteDto);
        }

    }
}