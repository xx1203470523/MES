/*
 *creator: Karl
 *
 *describe: 设备维修记录    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.MES.Services.Services.EquRepairOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquRepairOrder
{
    /// <summary>
    /// 控制器（设备维修记录）
    /// @author pengxin
    /// @date 2024-06-12 10:56:10
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquRepairOrderController : ControllerBase
    {
        /// <summary>
        /// 接口（设备维修记录）
        /// </summary>
        private readonly IEquRepairOrderService _equRepairOrderService;
        private readonly ILogger<EquRepairOrderController> _logger;

        /// <summary>
        /// 构造函数（设备维修记录）
        /// </summary>
        /// <param name="equRepairOrderService"></param>
        public EquRepairOrderController(IEquRepairOrderService equRepairOrderService, ILogger<EquRepairOrderController> logger)
        {
            _equRepairOrderService = equRepairOrderService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（设备维修记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquRepairOrderDto>> QueryPagedEquRepairOrderAsync([FromQuery] EquRepairOrderPagedQueryDto parm)
        {
            return await _equRepairOrderService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备维修记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquRepairOrderDto> QueryEquRepairOrderByIdAsync(long id)
        {
            return await _equRepairOrderService.QueryEquRepairOrderByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备维修记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquRepairOrderAsync([FromBody] EquRepairOrderCreateDto parm)
        {
             await _equRepairOrderService.CreateEquRepairOrderAsync(parm);
        }

        /// <summary>
        /// 更新（设备维修记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquRepairOrderAsync([FromBody] EquRepairOrderModifyDto parm)
        {
             await _equRepairOrderService.ModifyEquRepairOrderAsync(parm);
        }

        /// <summary>
        /// 删除（设备维修记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquRepairOrderAsync([FromBody] long[] ids)
        {
            await _equRepairOrderService.DeletesEquRepairOrderAsync(ids);
        }

        #endregion
    }
}