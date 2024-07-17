using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 控制器（条码追溯）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TracingSourceController : ControllerBase
    {
        /// <summary>
        /// 条码追溯服务接口
        /// </summary>
        private readonly ITracingSourceSFCService _tracingSFCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tracingSFCService"></param>
        public TracingSourceController(ITracingSourceSFCService tracingSFCService)
        {
            _tracingSFCService = tracingSFCService;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("from/{sfc}")]
        public async Task<NodeSourceDto> SourceAsync(string sfc)
        {
            return await _tracingSFCService.SourceAsync(sfc);
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("to/{sfc}")]
        public async Task<NodeSourceDto> DestinationAsync(string sfc)
        {
            return await _tracingSFCService.DestinationAsync(sfc);
        }
        /// <summary>
        /// 追溯条码工序相关信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("ProcedureSources/{sfc}")]
        public async Task<IEnumerable<ProcedureSourceDto>> GetProcedureSourcesAsync(string sfc)
        {
            return await _tracingSFCService.GetProcedureSourcesAsync(sfc);
        }
        /// <summary>
        /// 查询此条码所有的作业日志
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("StepSources/{sfc}")]
        public async Task<IEnumerable<StepSourceDto>> GetStepSourcesAsync(string sfc)
        {
            return await _tracingSFCService.GetStepSourcesAsync(sfc);
        }

        /// <summary>
        /// 查询此条码 产品参数 追溯
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("ProductParameterSources/{sfc}")]
        public async Task<IEnumerable<ProductParameterSourceDto>> GetProductParameterSourcesAsync(string sfc)
        {
            return await _tracingSFCService.GetProductParameterSourcesAsync(sfc);
        }
        /// <summary>
        /// 查询此条码原材料追溯信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("MaterialSources/{sfc}")]
        public async Task<IEnumerable<MaterialSourceDto>> GetMaterialSourcesAsync(string sfc)
        {
            return await _tracingSFCService.GetMaterialSourcesAsync(sfc);
        }


        /// <summary>
        /// 查询此条码生产过程中设备参数追溯信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("EquipmentParameterSources/{sfc}")]
        public async Task<IEnumerable<EquipmentParameterSourceDto>> GetEquipmentParameterSourcesAsync(string sfc)
        {
            return await _tracingSFCService.GetEquipmentParameterSourcesAsync(sfc);
        }
    }
}
