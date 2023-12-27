using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（操作面板）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class ManuFacePlateController : ControllerBase
    {
        /// <summary>
        /// 接口（操作面板）
        /// </summary>
        private readonly IManuFacePlateService _manuFacePlateService;
        private readonly ILogger<ManuFacePlateController> _logger;

        /// <summary>
        /// 构造函数（操作面板）
        /// </summary>
        /// <param name="manuFacePlateService"></param>
        /// <param name="logger"></param>
        public ManuFacePlateController(IManuFacePlateService manuFacePlateService, ILogger<ManuFacePlateController> logger)
        {
            _manuFacePlateService = manuFacePlateService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（操作面板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFacePlateDto>> QueryPagedManuFacePlateAsync([FromQuery] ManuFacePlatePagedQueryDto parm)
        {
            return await _manuFacePlateService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（操作面板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("queryManuFacePlateById/{id}")]
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByIdAsync(long id)
        {
            return await _manuFacePlateService.QueryManuFacePlateByIdAsync(id);
        }

        /// <summary>
        /// 添加（操作面板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("面板维护", BusinessType.INSERT)]
        [PermissionDescription("manu:facePlate:insert")]
        public async Task AddManuFacePlateAsync([FromBody] AddManuFacePlateDto parm)
        {
            await _manuFacePlateService.AddManuFacePlateAsync(parm);
        }

        /// <summary>
        /// 更新（操作面板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("面板维护", BusinessType.UPDATE)]
        [PermissionDescription("manu:facePlate:update")]
        public async Task UpdateManuFacePlateAsync([FromBody] UpdateManuFacePlateDto parm)
        {
            await _manuFacePlateService.UpdateManuFacePlateAsync(parm);
        }

        /// <summary>
        /// 删除（操作面板）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("面板维护", BusinessType.DELETE)]
        [PermissionDescription("manu:facePlate:delete")]
        public async Task DeleteManuFacePlateAsync([FromBody] long[] ids)
        {
            await _manuFacePlateService.DeletesManuFacePlateAsync(ids);
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 查询详情（操作面板）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("queryByCode/{code}")]
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByCodeAsync(string code)
        {
            return await _manuFacePlateService.QueryManuFacePlateByCodeAsync(code);
        }

        #region 状态变更

        /// <summary>
        /// 启用（操作面板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("操作面板", BusinessType.UPDATE)]
        [PermissionDescription("manu:facePlate:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _manuFacePlateService.UpdateManuFacePlateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（操作面板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("操作面板", BusinessType.UPDATE)]
        [PermissionDescription("manu:facePlate:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _manuFacePlateService.UpdateManuFacePlateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（上料点维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("上料点维护", BusinessType.UPDATE)]
        [PermissionDescription("manu:facePlate:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _manuFacePlateService.UpdateManuFacePlateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion

        #endregion
    }
}