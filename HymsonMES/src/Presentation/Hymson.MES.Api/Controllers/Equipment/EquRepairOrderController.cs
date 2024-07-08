/*
 *creator: Karl
 *
 *describe: 设备维修记录    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrderFault;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.EquRepairOrder;
using Hymson.Snowflake;
using Hymson.Web.Framework.Attributes;
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
        public async Task<EquRepairOrderFromDto> QueryEquRepairOrderByIdAsync(long id)
        {
            return await _equRepairOrderService.QueryEquRepairOrderByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（详细信息）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<EquRepairOrderFromDetailDto> QueryEquRepairOrderDetailByIdAsync(long id)
        {
            return await _equRepairOrderService.QueryEquRepairOrderDetailByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（根据OrderId查询故障详细）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("fault/{id}")]
        public async Task<IEnumerable<EquReportRepairFaultDto>> QueryEquRepairOrderFaultByOrderIdAsync(long id)
        {
            return await _equRepairOrderService.QueryEquRepairOrderFaultByOrderIdAsync(id);
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
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquRepairOrderAsync(EquRepairOrderDeletesDto param)
        {
            await _equRepairOrderService.DeletesEquRepairOrderAsync(param);
        }

        #endregion

        #region 操作


        /// <summary>
        /// 报修
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reportRepair")]
        public async Task ReportRepairAsync([FromBody] EquReportRepairDto parm)
        {
            await _equRepairOrderService.ReportRepairAsync(parm);
        }

        /// <summary>
        /// 维修
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("maintenance")]
        public async Task MaintenanceAsync([FromBody] EquMaintenanceDto parm)
        {
            await _equRepairOrderService.MaintenanceAsync(parm);
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirm")]
        public async Task ConfirmAsync([FromBody] ConfirmDto parm)
        {
            await _equRepairOrderService.ConfirmAsync(parm);
        }

        /// <summary>
        /// 获取Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("getNewId")]
        public long GetNewId()
        {
            return IdGenProvider.Instance.CreateId();
        }

        /// <summary>
        /// 上传单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        [LogDescription("上传单据附件", BusinessType.EXPORT)]
        public async Task SaveAttachmentAsync([FromBody] EquRepairOrderSaveAttachmentDto dto)
        {
            await _equRepairOrderService.SaveAttachmentAsync(dto);
        }

        /// <summary>
        /// 删除单据附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{orderAnnexId}")]
        [LogDescription("删除单据附件", BusinessType.OTHER)]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _equRepairOrderService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("attachment/attachmentList")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync([FromQuery] EquRepairOrderAttachmentDto dto)
        {
            return await _equRepairOrderService.QueryOrderAttachmentListByIdAsync(dto);
        }

        #endregion
    }
}