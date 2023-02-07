using Hymson.Infrastructure;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料维护）
    /// @tableName proc_material
    /// @author Czhipu
    /// @date 2022-08-29
    /// </summary>
    [ApiController]
    [Route("process/procMaterial")]
    public class ProcMaterialController : ControllerBase
    {
        //// <summary>
        /// 接口（物料维护）
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;
        private readonly ILogger<ProcMaterialController> _logger;

        /// <summary>
        /// 构造函数（物料维护）
        /// </summary>
        /// <param name="procMaterialService"></param>
        public ProcMaterialController(IProcMaterialService procMaterialService, ILogger<ProcMaterialController> logger)
        {
            _procMaterialService = procMaterialService;
            _logger = logger;
        }

        ///// <summary>
        ///// 获取分页数据
        ///// </summary>
        ///// <param name="whStockChangeRecordPagedQueryDto"></param>
        ///// <returns></returns>
        //[Route("pagelist")]
        //[HttpGet]
        //public async Task<PagedInfo<WhStockChangeRecordDto>> GetList([FromQuery]WhStockChangeRecordPagedQueryDto whStockChangeRecordPagedQueryDto)
        //{
        //    return await _whStockChangeRecordService.GetListAsync(whStockChangeRecordPagedQueryDto);
        //}

        ///// <summary>
        ///// 创建记录
        ///// </summary>
        ///// <param name="whStockChangeRecordDto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("create")]
        //public async Task Create(WhStockChangeRecordDto whStockChangeRecordDto)
        //{
        //    await _whStockChangeRecordService.CreateWhStockChangeRecordAsync(whStockChangeRecordDto);
        //}
    }
}