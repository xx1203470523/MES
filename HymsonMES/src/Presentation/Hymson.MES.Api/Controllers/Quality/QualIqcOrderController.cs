using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（IQC检验单）
    /// @author Czhipu
    /// @date 2024-03-06 02:04:09
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIqcOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIqcOrderController> _logger;

        /// <summary>
        /// 服务接口（IQC检验单）
        /// </summary>
        private readonly IQualIqcOrderService _qualIqcOrderService;

        /// <summary>
        /// 构造函数（OQC检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIqcOrderService"></param>
        public QualIqcOrderController(ILogger<QualIqcOrderController> logger, IQualIqcOrderService qualIqcOrderService)
        {
            _logger = logger;
            _qualIqcOrderService = qualIqcOrderService;
        }


        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpDelete("attachment/save")]
        public async Task SaveAttachmentAsync([FromBody] QualIqcOrderSaveAttachmentDto requestDto)
        {
            await _qualIqcOrderService.SaveAttachmentAsync(requestDto);
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete")]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _qualIqcOrderService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 删除（iqc检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteOrdersAsync([FromBody] long[] ids)
        {
            await _qualIqcOrderService.DeleteOrdersAsync(ids);
        }


        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("generated")]
        public async Task<long> GeneratedOrderAsync([FromBody] GenerateInspectionDto requestDto)
        {
            // TODO 生成IQC检验单
            return await Task.FromResult(0);
        }

        /// <summary>
        /// 更改检验单状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut("operation")]
        public async Task OperationOrderAsync([FromBody] QualOrderOperationStatusDto requestDto)
        {
            await _qualIqcOrderService.OperationOrderAsync(requestDto);
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<long> SaveOrderAsync([FromBody] QualIqcOrderSaveDto requestDto)
        {
            return await _qualIqcOrderService.SaveOrderAsync(requestDto);
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut("complete")]
        public async Task CompleteOrderAsync(QualIqcOrderCompleteDto requestDto)
        {
            await _qualIqcOrderService.CompleteOrderAsync(requestDto);
        }

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut("close")]
        public async Task CloseOrderAsync(QualIqcOrderCloseDto requestDto)
        {
            await _qualIqcOrderService.CloseOrderAsync(requestDto);
        }


        /// <summary>
        /// 查询详情（iqc检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIqcOrderDto?> QueryByIdAsync(long id)
        {
            return await _qualIqcOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（iqc检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<QualIqcOrderDto>> QueryPagedListAsync([FromQuery] QualIqcOrderPagedQueryDto pagedQueryDto)
        {
            return await _qualIqcOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询检验类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("type/{orderId}")]
        public async Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryTypeListByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryOrderTypeListByIdAsync(orderId);
        }

        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryOrderAttachmentListByIdAsync(orderId);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("snapshot")]
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSnapshotAsync([FromQuery] OrderParameterDetailQueryDto requestDto)
        {
            return await _qualIqcOrderService.QueryDetailSnapshotAsync(requestDto);
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("sample")]
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSampleAsync([FromQuery] OrderParameterDetailQueryDto requestDto)
        {
            return await _qualIqcOrderService.QueryDetailSampleAsync(requestDto);
        }


    }
}