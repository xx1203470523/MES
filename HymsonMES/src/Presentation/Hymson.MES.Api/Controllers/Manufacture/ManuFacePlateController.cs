/*
 *creator: Karl
 *
 *describe: 操作面板    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（操作面板）
    /// @author Karl
    /// @date 2023-04-01 02:05:24
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFacePlateController : ControllerBase
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
        [HttpGet("querymanufaceplatebyId/{id}")]
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
        #endregion
    }
}