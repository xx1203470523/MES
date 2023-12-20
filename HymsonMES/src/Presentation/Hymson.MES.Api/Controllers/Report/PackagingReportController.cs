using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（包装报告）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PackagingReportController : ControllerBase
    {
        /// <summary>
        /// 接口（包装报告）
        /// </summary>
        private readonly IPackagingReportService _packagingReportService;
        private readonly ILogger<PackagingReportController> _logger;

        /// <summary>
        /// 构造函数（包装报告）
        /// </summary>
        /// <param name="packagingReportService"></param>
        /// <param name="logger"></param>
        public PackagingReportController(IPackagingReportService packagingReportService, ILogger<PackagingReportController> logger)
        {
            _packagingReportService = packagingReportService;
            _logger = logger;
        }

        /// <summary>
        /// 根据容器编码或者装载条码查询容器当前信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("packing")]
        public async Task<ManuContainerBarcodeViewDto> QueryManuContainerByCodeAsync([FromQuery] PackagingQueryDto queryDto)
        {
            return await _packagingReportService.QueryManuContainerByCodeAsync(queryDto);
        }

        /// <summary>
        /// 查询工单包装信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orderPacking")]
        public async Task<PlanWorkPackDto> GetByWorkOrderCodeAsync([FromQuery] PackagingQueryDto queryDto)
        {
            return await _packagingReportService.GetByWorkOrderCodeAsync(queryDto);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("packPagelist")]
        public async Task<PagedInfo<ManuContainerPackDto>> GetContainerPackPagedListAsync([FromQuery] ManuContainerPackPagedQueryDto queryDto)
        {
            return await _packagingReportService.GetContainerPackPagedListAsync(queryDto);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orderPagelist")]
        public async Task<PagedInfo<PlanWorkPackingDto>> GetPagedListAsync([FromQuery] ManuContainerBarcodePagedQueryDto queryDto)
        {
            return await _packagingReportService.GetPagedListAsync(queryDto);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orderRecordPagelist")]
        public async Task<PagedInfo<PlanWorkPackingDto>> GetPagedRecordListAsync([FromQuery] ManuContainerBarcodePagedQueryDto queryDto)
        {
            return await _packagingReportService.GetPagedRecordListAsync(queryDto);
        }

    }
}