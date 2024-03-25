using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;
//using Hymson.MES.Services.Dtos.QualityOqcParameterGroup;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;

//using Hymson.MES.Services.Services.QualityOqcParameterGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualityOqcParameterGroup
{
    /// <summary>
    /// 控制器（OQC检验参数组）
    /// @author Jam
    /// @date 2024-03-07 01:47:16
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualOqcParameterGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualOqcParameterGroupController> _logger;
        /// <summary>
        /// 服务接口（OQC检验参数组）
        /// </summary>
        private readonly IQualOqcParameterGroupService _qualOqcParameterGroupService;


        /// <summary>
        /// 构造函数（OQC检验参数组）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualOqcParameterGroupService"></param>
        public QualOqcParameterGroupController(ILogger<QualOqcParameterGroupController> logger, IQualOqcParameterGroupService qualOqcParameterGroupService)
        {
            _logger = logger;
            _qualOqcParameterGroupService = qualOqcParameterGroupService;
        }

        /// <summary>
        /// OQC检验项目 ; 单条数据查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        [HttpGet("one")]
        [LogDescription("OQC检验项目获取单个", BusinessType.OTHER)]
        public async Task<QualOqcParameterGroupOutputDto> GetOneAsync([FromQuery] QualOqcParameterGroupQueryDto query)
        {
            return await _qualOqcParameterGroupService.GetOneAsync(query);
        }

        /// <summary>
        /// 添加（OQC检验参数组）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("OQC检验项目新增", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] QualOqcParameterGroupDto createDto)
        {
             await _qualOqcParameterGroupService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（OQC检验参数组）
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("OQC检验项目更新", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] QualOqcParameterGroupUpdateDto updateDto)
        {
             await _qualOqcParameterGroupService.ModifyAsync(updateDto);
        }

        /// <summary>
        /// 查询详情（OQC检验参数组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualOqcParameterGroupDto?> QueryByIdAsync(long id)
        {
            return await _qualOqcParameterGroupService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（OQC检验参数组）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualOqcParameterGroupDto>> QueryPagedListAsync([FromQuery] QualOqcParameterGroupPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcParameterGroupService.GetPagedListAsync(pagedQueryDto);
        }


        /// <summary>
        /// 分页查询-包含参数明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [LogDescription("查询OQC分页检验项目", BusinessType.OTHER)]
        public async Task<PagedInfo<QualOqcParameterGroupOutputDto>> GetPagedAsync([FromQuery] QualOqcParameterGroupPagedQueryDto query)
        {
            return await _qualOqcParameterGroupService.GetPagedAsync(query);
        }

        /// <summary>
        /// 删除OQC检验项目
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [LogDescription("删除OQC检验项目", BusinessType.DELETE)]
        public async Task DeleteSoftAsync(QualOqcParameterGroupDeleteDto deleteDto)
        {
            await _qualOqcParameterGroupService.DeleteAsync(deleteDto);
        }

    }
}