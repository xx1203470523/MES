using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（OQC检验单）
    /// @author xiaofei
    /// @date 2024-03-04 10:53:43
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualOqcOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualOqcOrderController> _logger;
        /// <summary>
        /// 服务接口（OQC检验单）
        /// </summary>
        private readonly IQualOqcOrderService _qualOqcOrderService;


        /// <summary>
        /// 构造函数（OQC检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualOqcOrderService"></param>
        public QualOqcOrderController(ILogger<QualOqcOrderController> logger, IQualOqcOrderService qualOqcOrderService)
        {
            _logger = logger;
            _qualOqcOrderService = qualOqcOrderService;
        }

        /// <summary>
        /// 添加（OQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualOqcOrderSaveDto saveDto)
        {
            await _qualOqcOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（OQC检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualOqcOrderSaveDto saveDto)
        {
            await _qualOqcOrderService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（OQC检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualOqcOrderService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（OQC检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualOqcOrderDto?> QueryByIdAsync(long id)
        {
            return await _qualOqcOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（OQC检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualOqcOrderDto>> QueryPagedListAsync([FromQuery] QualOqcOrderPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 获取OQC单价检验类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOqcOrderType/{id}")]
        public async Task<IEnumerable<QualOqcOrderTypeOutDto>> GetOqcOrderTypeAsync(long id)
        {
            return await _qualOqcOrderService.GetOqcOrderTypeAsync(id);
        }

        /// <summary>
        /// 校验条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkBarCode")]
        public async Task<IEnumerable<CheckBarCodeOutDto>> CheckBarCodeAsync([FromQuery] CheckBarCodeQuqryDto checkBarCodeQuqryDto)
        {
            return await _qualOqcOrderService.CheckBarCodeAsync(checkBarCodeQuqryDto);
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task SaveOrderAsync([FromBody] QualOqcOrderExecSaveDto requestDto)
        {
            await _qualOqcOrderService.SaveOrderAsync(requestDto);
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPut("complete")]
        public async Task CompleteOrderAsync(QualOqcOrderCompleteDto requestDto)
        {
            await _qualOqcOrderService.CompleteOrderAsync(requestDto);
        }

        /// <summary>
        /// 更新OQC检验单状态
        /// </summary>
        /// <param name="updateStatusDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatus")]
        public async Task UpdateStatusAsync([FromBody] UpdateStatusDto updateStatusDto)
        {
            await _qualOqcOrderService.UpdateStatusAsync(updateStatusDto);
        }

        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        public async Task<IEnumerable<OQCAnnexOutDto>> SaveAttachmentAsync([FromBody] QualOqcOrderSaveAttachmentDto requestDto)
        {
            return await _qualOqcOrderService.SaveAttachmentAsync(requestDto);
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{id}")]
        public async Task DeleteAttachmentByIdAsync(long id)
        {
            await _qualOqcOrderService.DeleteAttachmentByIdAsync(id);
        }

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("sample/pagelist")]
        public async Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryDetailSamplePagedListAsync([FromQuery] OqcOrderParameterDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcOrderService.OqcOrderQueryDetailSamplePagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 更新已检验
        /// </summary>
        /// <param name="updateSampleDetailDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateSampleDetail")]
        public async Task UpdateSampleDetailAsync([FromBody] UpdateSampleDetailDto updateSampleDetailDto)
        {
            await _qualOqcOrderService.UpdateSampleDetailAsync(updateSampleDetailDto);
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="oQCOrderUnqualifiedHandleDto"></param>
        /// <returns></returns>
        [HttpPost("unqualifiedHandle")]
        public async Task UnqualifiedHandleAsync([FromBody] OQCOrderUnqualifiedHandleDto oQCOrderUnqualifiedHandleDto)
        {
            await _qualOqcOrderService.UnqualifiedHandleAnync(oQCOrderUnqualifiedHandleDto);
        }

        /// <summary>
        /// 查询不合格样品数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("getUnqualified/pagelist")]
        public async Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryUnqualifiedPagedListAsync([FromQuery] OqcOrderParameterDetailPagedQueryDto pagedQueryDto)
        {
            return await _qualOqcOrderService.OqcOrderQueryUnqualifiedPagedListAsync(pagedQueryDto);
        }

        ///// <summary>
        ///// 查询检验类型
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //[HttpGet("type/{orderId}")]
        //public async Task<IEnumerable<QualOqcOrderTypeDto>> QueryTypeListByIdAsync(long orderId)
        //{
        //    return await _qualOqcOrderService.QueryOrderTypeListByIdAsync(orderId);
        //}

        /// <summary>
        /// 获取已检数据和样本数量
        /// </summary>
        /// <param name="sampleQtyAndCheckedQtyQueryDto"></param>
        /// <returns></returns>
        [HttpGet("getSampleQtyAndCheckedQty")]
        public async Task<SampleQtyAndCheckedQtyQueryOutDto> GetSampleQtyAndCheckedQtyAsync([FromQuery] SampleQtyAndCheckedQtyQueryDto sampleQtyAndCheckedQtyQueryDto) {
            return await _qualOqcOrderService.GetSampleQtyAndCheckedQtyAsync(sampleQtyAndCheckedQtyQueryDto);
        }
    }
}