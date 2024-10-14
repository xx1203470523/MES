using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.QualUnqualifiedCode;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 不合格代码控制器
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
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualUnqualifiedCodeDto>> QueryPagedQualUnqualifiedCodeAsync([FromQuery] QualUnqualifiedCodePagedQueryDto pagedQueryDto)
        {
            return await _qualUnqualifiedCodeService.GetPageListAsync(pagedQueryDto);
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
        [LogDescription("不合格代码", BusinessType.INSERT)]
        [PermissionDescription("qual:unqualifiedCode:insert")]
        public async Task<long> AddQualUnqualifiedCodeAsync([FromBody] QualUnqualifiedCodeCreateDto parm)
        {
            return await _qualUnqualifiedCodeService.CreateQualUnqualifiedCodeAsync(parm);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("不合格代码", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedCode:update")]
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
        [LogDescription("不合格代码", BusinessType.DELETE)]
        [PermissionDescription("qual:unqualifiedCode:delete")]
        public async Task DeleteQualUnqualifiedCodeAsync(long[] ids)
        {
            await _qualUnqualifiedCodeService.DeletesQualUnqualifiedCodeAsync(ids);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IEnumerable<SelectOptionDto>> QueryCodesAsync()
        {
            return await _qualUnqualifiedCodeService.QueryCodesAsync();
        }

        #region 状态变更
        /// <summary>
        /// 启用（不合格代码）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("不合格代码", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedCode:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _qualUnqualifiedCodeService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（不合格代码）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("不合格代码", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedCode:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _qualUnqualifiedCodeService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（不合格代码）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("不合格代码", BusinessType.UPDATE)]
        [PermissionDescription("qual:unqualifiedCode:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _qualUnqualifiedCodeService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}