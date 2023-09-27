using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Report;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器
    /// @author wxk
    /// @date 2023-04-12 02:29:23
    /// </summary>

    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuNgJudgeController : ControllerBase
    {
        /// <summary>
        /// 接口 
        /// </summary>
        private readonly IProductTraceReportService _productTraceReportService;
        private readonly ILogger<ManuNgJudgeController> _logger;

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="productTraceReportService"></param>
        public ManuNgJudgeController(IProductTraceReportService productTraceReportService, ILogger<ManuNgJudgeController> logger)
        {
            _productTraceReportService = productTraceReportService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 更新（NG判定）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        [LogDescription("NG判定", BusinessType.UPDATE)]
        public async Task UpdateManuContainerBarcodeAsync(UpdateNGJudgeDto parm)
        {
            try
            {
                await _productTraceReportService.UpdateNGJudgeAsync(parm);
            }
            catch (Exception ex)
            {
                int s = 1;
                throw ex;
            }
            
        }

        #endregion
    }
}
