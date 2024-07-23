using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（退料单）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuReturnOrderController : ControllerBase
    {
        /// <summary>
        /// 生产服务接口
        /// </summary>
        private readonly IManuReturnOrderService _manuReturnOrderService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuReturnOrderService"></param>
        public ManuReturnOrderController(IManuReturnOrderService manuReturnOrderService)
        {
            _manuReturnOrderService = manuReturnOrderService;
        }

        /// <summary>
        /// 分页查询列表（生产退料表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<ManuReturnOrderDto>> QueryPagedListAsync([FromQuery] ManuReturnOrderPagedQueryDto pagedQueryDto)
        {
            return await _manuReturnOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（生产退料表）
        /// </summary>
        /// <param name="returnId"></param>
        /// <returns></returns>
        [HttpGet("details/{returnId}")]
        public async Task<IEnumerable<ManuReturnOrderDetailDto>> QueryDetailByReturnIdAsync(long returnId)
        {
            return await _manuReturnOrderService.QueryDetailByReturnIdAsync(returnId);
        }

    }
}