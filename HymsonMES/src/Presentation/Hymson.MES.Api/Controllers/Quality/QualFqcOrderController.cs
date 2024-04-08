using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Hymson.MES.Services.Dtos.Quality.QualFqcParameterGroup;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（FQC检验单）
    /// @author Jam
    /// @date 2024-03-25 02:32:07
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualFqcOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualFqcOrderController> _logger;
        /// <summary>
        /// 服务接口（FQC检验单）
        /// </summary>
        private readonly IQualFqcOrderService _qualFqcOrderService;


        /// <summary>
        /// 构造函数（FQC检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualFqcOrderService"></param>
        public QualFqcOrderController(ILogger<QualFqcOrderController> logger, IQualFqcOrderService qualFqcOrderService)
        {
            _logger = logger;
            _qualFqcOrderService = qualFqcOrderService;
        }

        /// <summary>
        /// 添加（FQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> AddAsync([FromBody] QualFqcOrderCreateDto saveDto)
        {
             return await _qualFqcOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 测试条码产出时自动生成功能（FQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createtest")]
        public async Task<bool> AddAsync([FromBody] QualFqcOrderCreateTestDto saveDto)
        {
            return await _qualFqcOrderService.CreateAsync(saveDto);
        }

        

        /// <summary>
        /// 更新（FQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        //[HttpPut]
        //[Route("update")]
        //public async Task UpdateAsync([FromBody] QualFqcOrderSaveDto saveDto)
        //{
        //     await _qualFqcOrderService.ModifyAsync(saveDto);
        //}

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<int> UpdateOrderAsync(FQCParameterDetailSaveDto requestDto)
        {
            return await _qualFqcOrderService.UpdateOrderAsync(requestDto);
        }

        /// <summary>
        /// 删除（FQC检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualFqcOrderService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（FQC检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualFqcOrderDto?> QueryByIdAsync(long id)
        {
            return await _qualFqcOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（FQC检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualFqcOrderDto>> QueryPagedListAsync([FromQuery] QualFqcOrderPagedQueryDto pagedQueryDto)
        {
            return await _qualFqcOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 上传检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        public async Task SaveAttachmentAsync([FromBody] QualFqcOrderSaveAttachmentDto requestDto)
        {
            await _qualFqcOrderService.SaveAttachmentAsync(requestDto);
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{orderAnnexId}")]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _qualFqcOrderService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 生成FQC检验单
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost("generated")]
        public async Task<int> GeneratedOrderAsync([FromBody] QualFqcOrderCreateDto saveDto)
        {
            return await _qualFqcOrderService.CreateAsync(saveDto);
            // TODO 生成IQC检验单
            //return await _qualFqcOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("operation")]
        public async Task<long> OperationOrderAsync([FromBody] OrderOperationStatusDto requestDto)
        {
            return await _qualFqcOrderService.OperationOrderAsync(requestDto);
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<long> SaveOrderAsync([FromBody] QualFqcOrderSampleSaveDto requestDto)
        {
            return await _qualFqcOrderService.SaveOrderAsync(requestDto);
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("complete")]
        public async Task<long> CompleteOrderAsync(QualFqcOrderCompleteDto requestDto)
        {
            return await _qualFqcOrderService.CompleteOrderAsync(requestDto);
        }

        /// <summary>
        /// 检验单免检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //[HttpPost("free")]
        //public async Task<long> FreeOrderAsync(QualIqcOrderFreeDto requestDto)
        //{
        //    return await _qualFqcOrderService.FreeOrderAsync(requestDto);
        //}

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("close")]
        public async Task<long> CloseOrderAsync(QualFqcOrderCloseDto requestDto)
        {
            return await _qualFqcOrderService.CloseOrderAsync(requestDto);
        }

        /// <summary>
        /// 查询检验类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //[HttpGet("type/{orderId}")]
        //public async Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryTypeListByIdAsync(long orderId)
        //{
        //    return await _qualFqcOrderService.QueryOrderTypeListByIdAsync(orderId);
        //}

        /// <summary>
        /// 查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("attachment/{orderId}")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync(long orderId)
        {
            return await _qualFqcOrderService.QueryOrderAttachmentListByIdAsync(orderId);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("snapshot")]
        public async Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSnapshotAsync([FromQuery] FQCParameterDetailQueryDto requestDto)
        {
            return await _qualFqcOrderService.QueryDetailSnapshotAsync(requestDto);
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("sample")]
        public async Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSampleAsync([FromQuery] FQCParameterDetailQueryDto requestDto)
        {
            return await _qualFqcOrderService.QueryDetailSampleAsync(requestDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet("verfyParametergroup")]
        public async Task<IEnumerable<QualFqcParameterGroupEntity>> verificationParametergroupAsync([FromQuery] ParameterGroupQuery requestDto)
        {
            return await _qualFqcOrderService.VerificationParametergroupAsync(requestDto);
        }
 
        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("sample/pagelist")]
        public async Task<PagedInfo<FQCParameterDetailDto>> QueryDetailSamplePagedListAsync([FromQuery] FQCParameterDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualFqcOrderService.QueryDetailSamplePagedListAsync(pagedQueryDto);
        }

    }
}