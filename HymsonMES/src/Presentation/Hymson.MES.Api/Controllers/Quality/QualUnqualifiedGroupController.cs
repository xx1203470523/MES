
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（不合格代码组）
    /// @author wangkeming
    /// @date 2023-02-13 02:05:50
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualUnqualifiedGroupController : ControllerBase
    {
        private readonly IQualUnqualifiedGroupService _qualUnqualifiedGroupService;
        private readonly ILogger<QualUnqualifiedGroupController> _logger;

        /// <summary>
        /// 构造函数（不合格代码组）
        /// </summary>
        /// <param name="qualUnqualifiedGroupService"></param>
        public QualUnqualifiedGroupController(IQualUnqualifiedGroupService qualUnqualifiedGroupService, ILogger<QualUnqualifiedGroupController> logger)
        {
            _qualUnqualifiedGroupService = qualUnqualifiedGroupService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（不合格代码组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<QualUnqualifiedGroupDto>> QueryPagedQualUnqualifiedGroupAsync([FromQuery] QualUnqualifiedGroupPagedQueryDto parm)
        {
            return await _qualUnqualifiedGroupService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（不合格代码组）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualUnqualifiedGroupDto> QueryQualUnqualifiedGroupByIdAsync(long id)
        {
            return await _qualUnqualifiedGroupService.QueryQualUnqualifiedGroupByIdAsync(id);
        }

        /// <summary>
        /// 添加（不合格代码组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddQualUnqualifiedGroupAsync([FromBody] QualUnqualifiedGroupCreateDto parm)
        {
            await _qualUnqualifiedGroupService.CreateQualUnqualifiedGroupAsync(parm);
        }

        /// <summary>
        /// 更新（不合格代码组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateQualUnqualifiedGroupAsync([FromBody] QualUnqualifiedGroupModifyDto parm)
        {
            await _qualUnqualifiedGroupService.ModifyQualUnqualifiedGroupAsync(parm);
        }

        /// <summary>
        /// 删除（不合格代码组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteQualUnqualifiedGroupAsync(string ids)
        {
            await _qualUnqualifiedGroupService.DeletesQualUnqualifiedGroupAsync(ids);
        }
    }
}