using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.QualUnqualifiedGroup;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（不合格代码组）
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
        /// <param name="logger"></param>
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
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualUnqualifiedGroupDto>> QueryPagedQualUnqualifiedGroupAsync([FromQuery] QualUnqualifiedGroupPagedQueryDto parm)
        {
            return await _qualUnqualifiedGroupService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listByProcedure")]
        [HttpGet]
        public async Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByProcedureIdAsync([FromQuery] QualUnqualifiedGroupQueryDto query)
        {
            return await _qualUnqualifiedGroupService.GetListByProcedureIdAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listByMaterialGroup")]
        [HttpGet]
        public async Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByMaterialGroupIddAsync([FromQuery] QualUnqualifiedGroupQueryDto query)
        {
            return await _qualUnqualifiedGroupService.GetListByMaterialGroupIddAsync(query);
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
        /// 查询详情（不合格代码组工序）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/unqualifiedCodeList")]
        public async Task<List<QualUnqualifiedGroupCodeRelationDto>> QueryQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            return await _qualUnqualifiedGroupService.GetQualUnqualifiedCodeGroupRelationByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（不合格代码组不合格代码）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/procedureList")]
        public async Task<List<QualUnqualifiedGroupProcedureRelationDto>> QueryQualUnqualifiedCodeProcedureRelationpByIdAsync(long id)
        {
            return await _qualUnqualifiedGroupService.GetQualUnqualifiedCodeProcedureRelationByIdAsync(id);
        }

        /// <summary>
        /// 添加（不合格代码组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("不合格代码组", BusinessType.INSERT)]
        [PermissionDescription("qual:unqualifiedGroup:insert")]
        public async Task<long> AddQualUnqualifiedGroupAsync([FromBody] QualUnqualifiedGroupCreateDto parm)
        {
            return await _qualUnqualifiedGroupService.CreateQualUnqualifiedGroupAsync(parm);
        }
   
        /// <summary>
        /// 更新（不合格代码组）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("不合格代码组", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedGroup:update")]
        public async Task UpdateQualUnqualifiedGroupAsync([FromBody] QualUnqualifiedGroupModifyDto parm)
        {
            await _qualUnqualifiedGroupService.ModifyQualUnqualifiedGroupAsync(parm);
        }

        /// <summary>
        /// 删除（不合格代码组）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("不合格代码组", BusinessType.DELETE)]
        [PermissionDescription("qual:unqualifiedGroup:delete")]
        public async Task DeleteQualUnqualifiedGroupAsync(long[] ids)
        {
            await _qualUnqualifiedGroupService.DeletesQualUnqualifiedGroupAsync(ids);
        }
    }
}