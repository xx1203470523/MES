using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（废成品入库记录）
    /// @author User
    /// @date 2024-08-14 04:39:04
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuWasteProductsReceiptRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuWasteProductsReceiptRecordController> _logger;
        /// <summary>
        /// 服务接口（废成品入库记录）
        /// </summary>
        private readonly IManuWasteProductsReceiptRecordService _manuWasteProductsReceiptRecordService;


        /// <summary>
        /// 构造函数（废成品入库记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuWasteProductsReceiptRecordService"></param>
        public ManuWasteProductsReceiptRecordController(ILogger<ManuWasteProductsReceiptRecordController> logger, IManuWasteProductsReceiptRecordService manuWasteProductsReceiptRecordService)
        {
            _logger = logger;
            _manuWasteProductsReceiptRecordService = manuWasteProductsReceiptRecordService;
        }

        /// <summary>
        /// 添加（废成品入库记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuWasteProductsReceiptRecordSaveDto saveDto)
        {
             await _manuWasteProductsReceiptRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（废成品入库记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuWasteProductsReceiptRecordSaveDto saveDto)
        {
             await _manuWasteProductsReceiptRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（废成品入库记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuWasteProductsReceiptRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（废成品入库记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuWasteProductsReceiptRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuWasteProductsReceiptRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（废成品入库记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuWasteProductsReceiptRecordDto>> QueryPagedListAsync([FromQuery] ManuWasteProductsReceiptRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuWasteProductsReceiptRecordService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 分页查询列表（工单完工入库明细）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("WasteProductReceiptOrderDetail")]
        public async Task<IEnumerable<ManuWasteProductsReceiptRecordDto>> QueryPagedListAsync()
        {
            return await _manuWasteProductsReceiptRecordService.QueryWasteProductsReceiptInfoListAsync();
        }
    }
}