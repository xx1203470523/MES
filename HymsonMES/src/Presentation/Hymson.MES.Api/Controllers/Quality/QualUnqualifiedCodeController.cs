using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Hymson.Utils.Extensions;
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
        [HttpPost]
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
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddQualUnqualifiedCodeAsync([FromBody] QualUnqualifiedCodeCreateDto parm)
        {
            await _qualUnqualifiedCodeService.CreateQualUnqualifiedCodeAsync(parm);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateQualUnqualifiedCodeAsync([FromBody] QualUnqualifiedCodeModifyDto parm)
        {
            await _qualUnqualifiedCodeService.ModifyQualUnqualifiedCodeAsync(parm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteQualUnqualifiedCodeAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _qualUnqualifiedCodeService.DeletesQualUnqualifiedCodeAsync(ids);
        }
    }
}