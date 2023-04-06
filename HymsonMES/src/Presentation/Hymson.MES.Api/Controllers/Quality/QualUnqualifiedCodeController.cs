using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 不合格代码控制器
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualUnqualifiedCodeController : ControllerBase
    {
        private readonly IQualUnqualifiedCodeService _qualUnqualifiedCodeService;
        private readonly ILogger<QualUnqualifiedCodeController> _logger;

        /// <summary>
        /// 不合格代码控制器
        /// </summary>
        /// <param name="qualUnqualifiedCodeService"></param>
        /// <param name="logger"></param>
        public QualUnqualifiedCodeController(IQualUnqualifiedCodeService qualUnqualifiedCodeService, ILogger<QualUnqualifiedCodeController> logger)
        {
            _qualUnqualifiedCodeService = qualUnqualifiedCodeService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualUnqualifiedCodeDto>> QueryPagedQualUnqualifiedCodeAsync([FromQuery] QualUnqualifiedCodePagedQueryDto parm)
        {
            return await _qualUnqualifiedCodeService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualUnqualifiedCodeDto> QueryQualUnqualifiedCodeByIdAsync(long id)
        {
            return await _qualUnqualifiedCodeService.QueryQualUnqualifiedCodeByIdAsync(id);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("listByIds")]
        public async Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByIdsAsync([FromQuery] QualUnqualifiedCodeQueryDto queryDto)
        {
            return await _qualUnqualifiedCodeService.GetListByIdsAsync(queryDto.Ids);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/unqualifiedGroupList")]
        public async Task<List<UnqualifiedCodeGroupRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            return await _qualUnqualifiedCodeService.GetQualUnqualifiedCodeGroupRelationByIdAsync(id);
        }

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("listByGroupId/{id}")]
        [HttpGet]
        public async Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByGroupIdAsync(long id)
        {
            return await _qualUnqualifiedCodeService.GetListByGroupIdAsync(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddQualUnqualifiedCodeAsync([FromBody] QualUnqualifiedCodeCreateDto parm)
        {
            await _qualUnqualifiedCodeService.CreateQualUnqualifiedCodeAsync(parm);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateQualUnqualifiedCodeAsync([FromBody] QualUnqualifiedCodeModifyDto parm)
        {
            await _qualUnqualifiedCodeService.ModifyQualUnqualifiedCodeAsync(parm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteQualUnqualifiedCodeAsync(long[] ids)
        {
            await _qualUnqualifiedCodeService.DeletesQualUnqualifiedCodeAsync(ids);
        }
    }
}