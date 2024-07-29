using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceTask;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备保养任务）
    /// @author JAM
    /// @date 2024-05-23 03:20:49
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquMaintenanceTaskController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquMaintenanceTaskController> _logger;
        /// <summary>
        /// 服务接口（设备保养任务）
        /// </summary>
        private readonly IEquMaintenanceTaskService _equMaintenanceTaskService;


        /// <summary>
        /// 构造函数（设备保养任务）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equMaintenanceTaskService"></param>
        public EquMaintenanceTaskController(ILogger<EquMaintenanceTaskController> logger, IEquMaintenanceTaskService equMaintenanceTaskService)
        {
            _logger = logger;
            _equMaintenanceTaskService = equMaintenanceTaskService;
        }

        /// <summary>
        /// 添加（设备保养任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquMaintenanceTaskSaveDto saveDto)
        {
             await _equMaintenanceTaskService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备保养任务）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquMaintenanceTaskSaveDto saveDto)
        {
             await _equMaintenanceTaskService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备保养任务）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equMaintenanceTaskService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备保养任务）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquMaintenanceTaskDto?> QueryByIdAsync(long id)
        {
            return await _equMaintenanceTaskService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备保养任务）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquMaintenanceTaskDto>> QueryPagedListAsync([FromQuery] EquMaintenanceTaskPagedQueryDto pagedQueryDto)
        {
            return await _equMaintenanceTaskService.GetPagedListAsync(pagedQueryDto);
        }


        /// <summary>
        /// 更改单据状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("operation")]
        [LogDescription("更改单据状态", BusinessType.OTHER)]
        public async Task<long> OperationOrderAsync([FromBody] EquMaintenanceTaskOrderOperationStatusDto requestDto)
        {
            return await _equMaintenanceTaskService.OperationOrderAsync(requestDto);
        }


        /// <summary>
        /// 查询点检单明细项数据(执行-查询)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("snapshot")]
        public async Task<IEnumerable<EquMaintenanceTaskItemUnionSnapshotView>> querySnapshotItemAsync([FromQuery] EquMaintenanceTaskSnapshotItemQueryDto requestDto)
        {
            return await _equMaintenanceTaskService.querySnapshotItemAsync(requestDto);
        }

        /// <summary>
        /// 查询明细数据(结果处理)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("sample/pagelist")]
        public async Task<PagedInfo<EquMaintenanceTaskItemUnionSnapshotView>> QueryItemPagedListAsync([FromQuery] EquMaintenanceTaskItemPagedQueryDto dto)
        {
            return await _equMaintenanceTaskService.QueryItemPagedListAsync(dto);
        }

        /// <summary>
        /// 保存明细项(执行-保存)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [LogDescription("保存明细项", BusinessType.OTHER)]
        public async Task<long> SaveOrderAsync([FromBody] EquMaintenanceTaskItemSaveDto requestDto)
        {
            return await _equMaintenanceTaskService.SaveAndUpdateTaskItemAsync(requestDto);
        }

        /// <summary>
        /// 完成明细检验单(执行-完成)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("complete")]
        [LogDescription("完成检验单", BusinessType.OTHER)]
        public async Task<long> CompleteOrderAsync(EquMaintenanceTaskCompleteDto requestDto)
        {
            return await _equMaintenanceTaskService.CompleteOrderAsync(requestDto);
        }


        /// <summary>
        /// 结果处理(首页-处理)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("close")]
        [LogDescription("结果处理", BusinessType.OTHER)]
        public async Task<long> CloseOrderAsync(EquMaintenanceTaskCloseDto requestDto)
        {
            return await _equMaintenanceTaskService.CloseOrderAsync(requestDto);
        }

        /// <summary>
        /// 保养延期
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("defer")]
        [LogDescription("保养延期", BusinessType.OTHER)]
        public async Task<long> DeferOrderAsync(EquMaintenanceTaskDeferDto requestDto)
        {
            return await _equMaintenanceTaskService.DeferOrderAsync(requestDto);
        }

        /// <summary>
        /// 上传单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        [LogDescription("上传单据附件", BusinessType.EXPORT)]
        public async Task SaveAttachmentAsync([FromBody] EquMaintenanceTaskSaveAttachmentDto dto)
        {
            await _equMaintenanceTaskService.SaveAttachmentAsync(dto);
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
            await _equMaintenanceTaskService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _equMaintenanceTaskService.QueryOrderAttachmentListByIdAsync(orderId);
        }

    }
}