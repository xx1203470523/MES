/*
 *creator: Karl
 *
 *describe: 供应商    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Web.Framework.Attributes;
//using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（供应商）
    /// @author pengxin
    /// @date 2023-03-03 01:51:43
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhSupplierController : ControllerBase
    {
        /// <summary>
        /// 接口（供应商）
        /// </summary>
        private readonly IWhSupplierService _whSupplierService;
        private readonly ILogger<WhSupplierController> _logger;

        /// <summary>
        /// 构造函数（供应商）
        /// </summary>
        /// <param name="whSupplierService"></param>
        public WhSupplierController(IWhSupplierService whSupplierService, ILogger<WhSupplierController> logger)
        {
            _whSupplierService = whSupplierService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<WhSupplierDto>> QueryPagedWhSupplierAsync(WhSupplierPagedQueryDto parm)
        {
            return await _whSupplierService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（供应商）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhSupplierDto> QueryWhSupplierByIdAsync(long id)
        {
            return await _whSupplierService.QueryWhSupplierByIdAsync(id);
        }

        /// <summary>
        /// 根据ID查询(更改供应商)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("updatedto/{id}")]
        public async Task<UpdateWhSupplierDto> QueryUpdateWhSupplierByIdAsync(long id)
        {
            return await _whSupplierService.QueryUpdateWhSupplierByIdAsync(id);
        }

        /// <summary>
        /// 添加（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("供应商管理", BusinessType.INSERT)]
        [PermissionDescription("wh:supplier:insert")]
        public async Task AddWhSupplierAsync([FromBody] WhSupplierCreateDto parm)
        {
            await _whSupplierService.CreateWhSupplierAsync(parm);
        }

        /// <summary>
        /// 更新（供应商）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("供应商管理", BusinessType.UPDATE)]
        [PermissionDescription("wh:supplier:update")]
        public async Task UpdateWhSupplierAsync([FromBody] WhSupplierModifyDto parm)
        {
            await _whSupplierService.ModifyWhSupplierAsync(parm);
        }

        /// <summary>
        /// 删除（供应商）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("供应商管理", BusinessType.DELETE)]
        [PermissionDescription("wh:supplier:delete")]
        public async Task DeleteWhSupplierAsync([FromBody] long[] ids)
        {
            await _whSupplierService.DeletesWhSupplierAsync(ids);
        }

    }
}