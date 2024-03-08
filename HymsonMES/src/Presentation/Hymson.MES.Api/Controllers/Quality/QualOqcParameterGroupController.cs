using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
//using Hymson.MES.Services.Dtos.QualityOqcParameterGroup;
using Hymson.MES.Services.Services.Quality;
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
        /// 添加（OQC检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualOqcParameterGroupSaveDto saveDto)
        {
             await _qualOqcParameterGroupService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（OQC检验参数组）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualOqcParameterGroupSaveDto saveDto)
        {
             await _qualOqcParameterGroupService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（OQC检验参数组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualOqcParameterGroupService.DeletesAsync(ids);
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

    }
}