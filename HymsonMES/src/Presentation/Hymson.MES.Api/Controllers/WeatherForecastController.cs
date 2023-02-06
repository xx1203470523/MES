using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Services.OnStock;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhStockChangeRecordController : ControllerBase
    {
       /// <summary>
       /// 
       /// </summary>
        private readonly IWhStockChangeRecordService _whStockChangeRecordService;
        private readonly ILogger<WhStockChangeRecordController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whStockChangeRecordService"></param>
        /// <param name="logger"></param>
        public WhStockChangeRecordController(IWhStockChangeRecordService  whStockChangeRecordService,ILogger<WhStockChangeRecordController> logger)
        {
            _whStockChangeRecordService = whStockChangeRecordService;
            _logger = logger;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="whStockChangeRecordPagedQueryDto"></param>
        /// <returns></returns>
        [Route("pagelist")]
        [HttpGet]
        public async Task<PagedInfo<WhStockChangeRecordDto>> GetList([FromQuery]WhStockChangeRecordPagedQueryDto whStockChangeRecordPagedQueryDto)
        {
            return await _whStockChangeRecordService.GetListAsync(whStockChangeRecordPagedQueryDto);
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="whStockChangeRecordDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task Create(WhStockChangeRecordDto whStockChangeRecordDto)
        {
            await _whStockChangeRecordService.CreateWhStockChangeRecordAsync(whStockChangeRecordDto);
        }
    }
}