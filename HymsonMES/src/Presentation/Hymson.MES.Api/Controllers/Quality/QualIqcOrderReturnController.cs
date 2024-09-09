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
    public class QualIqcOrderReturnController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIqcOrderReturnController> _logger;

        /// <summary>
        /// 服务接口（IQC检验单）
        /// </summary>
        private readonly IQualIqcOrderReturnService _qualIqcOrderReturnService;

        /// <summary>
        /// 构造函数（IQC检验水平）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIqcOrderReturnService"></param>
        public QualIqcOrderReturnController(ILogger<QualIqcOrderReturnController> logger, IQualIqcOrderReturnService qualIqcOrderReturnService)
        {
            _logger = logger;
            _qualIqcOrderReturnService = qualIqcOrderReturnService;
        }


        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("generated")]
        [LogDescription("生成IQC检验单", BusinessType.INSERT)]
        public async Task<long> GeneratedOrderAsync([FromBody] GenerateOrderReturnDto requestDto)
        {
            // 生成IQC检验单
            return await _qualIqcOrderReturnService.GeneratedOrderAsync(requestDto);
        }

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("operation")]
        [LogDescription("更改检验单状态", BusinessType.INSERT)]
        public async Task<long> OperationOrderAsync([FromBody] QualOrderReturnOperationStatusDto requestDto)
        {
            return await _qualIqcOrderReturnService.OperationOrderAsync(requestDto);
        }


        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [LogDescription("保存样品数据", BusinessType.OTHER)]
        public async Task<long> SaveOrderAsync([FromBody] QualIqcOrderReturnSaveDto requestDto)
        {
            return await _qualIqcOrderReturnService.SaveOrderAsync(requestDto);
        }


        /// <summary>
        /// 上传检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        [LogDescription("IQC检验水平", BusinessType.OTHER)]
        public async Task SaveAttachmentAsync([FromBody] QualIqcOrderSaveAttachmentDto requestDto)
        {
            await _qualIqcOrderReturnService.SaveAttachmentAsync(requestDto);
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{orderAnnexId}")]
        [LogDescription("IQC检验水平", BusinessType.OTHER)]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _qualIqcOrderReturnService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 删除（IQC检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [LogDescription("删除IQC检验单", BusinessType.DELETE)]
        public async Task DeleteOrdersAsync([FromBody] long[] ids)
        {
            await _qualIqcOrderReturnService.DeleteOrdersAsync(ids);
        }


        /// <summary>
        /// 查询详情（iqc检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIqcOrderReturnBaseDto?> QueryByIdAsync(long id)
        {
            return await _qualIqcOrderReturnService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 取消（iqc检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("cancel/{id}")]
        public async Task<int> CancelOrderAsync(long id)
        {
            return await _qualIqcOrderReturnService.CancelOrderAsync(id);
        }

        /// <summary>
        /// 查询详情（iqc检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public async Task<IEnumerable<QualIqcOrderReturnDetailDto>?> QueryOrderDetailAsync(long id)
        {
            return await _qualIqcOrderReturnService.QueryOrderDetailAsync(id);
        }

        /// <summary>
        /// 分页查询列表（iqc检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<QualIqcOrderReturnDto>> QueryPagedListAsync([FromQuery] QualIqcOrderReturnPagedQueryDto pagedQueryDto)
        {
            return await _qualIqcOrderReturnService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _qualIqcOrderReturnService.QueryOrderAttachmentListByIdAsync(orderId);
        }

    }
}