using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（包装报告）
    /// @author zhaoqing
    /// @date 2023-04-21 17:34:17
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductDetailReportController : ControllerBase
    {
        #region 服务层
        private readonly ILogger<PackagingReportController> _logger;
        private readonly IProductDetailService _productDetailService;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productDetailService"></param>
        /// <param name="logger"></param>
        public ProductDetailReportController(IProductDetailService productDetailService, ILogger<PackagingReportController> logger)
        {
            _productDetailService = productDetailService;
            _logger = logger;
        }

        /// <summary>
        /// 报表-产能报表：分页获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<ProductDetailReportOutputDto>> GetPageInfoAsync([FromQuery]ProductDetailReportPageQueryDto pageQueryDto)
        {
            return await _productDetailService.GetPageInfoAsync(pageQueryDto);
        }

        /// <summary>
        /// 报表-产能报表：导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<ExportResultDto> ExportExcelAsync([FromQuery] ProductDetailReportPageQueryDto pageQueryDto)
        {
            return await _productDetailService.ExportExcelAsync(pageQueryDto);
        }

        /// <summary>
        /// 获取下线产出数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getOutputSum")]
        public async Task<decimal> GetOutputSumQtyAsync([FromQuery] ProductDetailReportQueryDto query)
        {
            return await _productDetailService.GetOutputQtyAsync(query);
        }

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        [HttpGet("shops")]
        public async Task<IEnumerable<SelectOptionDto>> GetProcShopListAsync()
        {
            return await _productDetailService.GetProcdureListAsync();
        }
    }
}
