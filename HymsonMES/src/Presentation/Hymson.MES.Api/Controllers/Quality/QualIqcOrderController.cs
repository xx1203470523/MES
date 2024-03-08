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
        /// 删除（iqc检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIqcOrderService.DeletesAsync(ids);
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
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIqcOrderDto>> QueryPagedListAsync([FromQuery] QualIqcOrderPagedQueryDto pagedQueryDto)
        {
            return await _qualIqcOrderService.GetPagedListAsync(pagedQueryDto);
        }



        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generated")]
        public async Task<long> GeneratedAsync([FromBody] GenerateInspectionDto saveDto)
        {
            // TODO 生成IQC检验单
            return await Task.FromResult(0);
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orderNoPageList")]
        public async Task<PagedInfo<ReceiptMaterialDto>> GetPagedListAsync([FromQuery] ReceiptMaterialPagedQueryDto pagedQueryDto)
        {
            await Task.CompletedTask;
            return new PagedInfo<ReceiptMaterialDto>(new List<ReceiptMaterialDto> {
                new ReceiptMaterialDto() {
                    Id = 1,
                    ReceiptNum = "56",
                    SupplierId = 38964670333591552,
                    SupplierCode = "GYS001",
                    SupplierName = "供应商001",
                    Remark = "备注",
                    UpdatedBy = "Czhipu",
                    UpdatedOn = DateTime.Now
                }
            }, 1, 10, 1);
        }

        /// <summary>
        /// 根据收货单号查询收货单明细
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet("orderNo/{orderNo}")]
        public async Task<IEnumerable<ReceiptMaterialItemDto>> QueryByOrderNoItemListAsync(string orderNo)
        {
            await Task.CompletedTask;
            var entity = new ReceiptMaterialItemDto()
            {
                Id = 1,
                ReceiptNum = "56",
                SupplierId = 38964670333591552,
                SupplierCode = "GYS001",
                SupplierName = "供应商001",
                SupplierBatch = "1",
                MaterialId = 38269647132454912,
                MaterialCode = "WL001",
                MaterialName = "物料001",
                MaterialVersion = "V1.0",
                InternalBatch = "2",
                PlanQty = 99,
                PlanTime = DateTime.Now
            };

            return new List<ReceiptMaterialItemDto>() { entity };
        }

        /// <summary>
        /// 查询检验类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("type/{orderId}")]
        public async Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryTypeListByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryTypeListByIdAsync(orderId);
        }

        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryAttachmentListByIdAsync(orderId);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("snapshot/{orderId}")]
        public async Task<IEnumerable<InspectionParameterDetailDto>> QueryDetailSnapshotByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryDetailSnapshotByIdAsync(orderId);
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("sample/{orderId}")]
        public async Task<IEnumerable<InspectionParameterDetailDto>> QueryDetailSampleByIdAsync(long orderId)
        {
            return await _qualIqcOrderService.QueryDetailSampleByIdAsync(orderId);
        }

        // TODO 保存

        // TODO 完成

    }
}