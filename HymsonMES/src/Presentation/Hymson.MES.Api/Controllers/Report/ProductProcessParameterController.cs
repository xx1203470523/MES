using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.ProductProcessParameter;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（产品过程参数）
    /// @author zhaoqing
    /// @date 2023-10-13 10:14:17
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductProcessParameterController : ControllerBase
    {
        /// <summary>
        /// 接口（产品过程参数）
        /// </summary>
        private readonly IProductProcessParameterService _processParameterService;
        private readonly ILogger<ProductProcessParameterController> _logger;

        /// <summary>
        /// 构造函数（产品过程参数）
        /// </summary>
        /// <param name="processParameterService"></param>
        /// <param name="logger"></param>
        public ProductProcessParameterController(IProductProcessParameterService processParameterService, ILogger<ProductProcessParameterController> logger)
        {
            _processParameterService = processParameterService;
            _logger = logger;
        }

        /// <summary>
        /// 查询列表（产品过程参数）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ProductProcessParameterReportDto>> GetPageListAsync([FromQuery] ProductProcessParameterReportPagedQueryDto queryDto)
        {
            return await _processParameterService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 导出产品过程参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("export")]
        public async Task<ProductProcessParameterExportResultDto> ExprotEquProcessParameterListAsync([FromQuery] ProductProcessParameterReportPagedQueryDto param)
        {
            return await _processParameterService.ExprotProductProcessParameterListAsync(param);
        }
    }
}