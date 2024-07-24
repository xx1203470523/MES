using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（工单完工入库）
    /// @author User
    /// @date 2024-07-17 10:29:03
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductReceiptOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuProductReceiptOrderController> _logger;
        /// <summary>
        /// 服务接口（工单完工入库）
        /// </summary>
        private readonly IManuProductReceiptOrderService _manuProductReceiptOrderService;


        /// <summary>
        /// 构造函数（工单完工入库）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuProductReceiptOrderService"></param>
        public ManuProductReceiptOrderController(ILogger<ManuProductReceiptOrderController> logger, IManuProductReceiptOrderService manuProductReceiptOrderService)
        {
            _logger = logger;
            _manuProductReceiptOrderService = manuProductReceiptOrderService;
        }

        /// <summary>
        /// 添加（工单完工入库）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuProductReceiptOrderSaveDto saveDto)
        {
             await _manuProductReceiptOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（工单完工入库）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuProductReceiptOrderSaveDto saveDto)
        {
             await _manuProductReceiptOrderService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（工单完工入库）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuProductReceiptOrderService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（工单完工入库）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuProductReceiptOrderDto?> QueryByIdAsync(long id)
        {
            return await _manuProductReceiptOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（工单完工入库）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuProductReceiptOrderDto>> QueryPagedListAsync([FromQuery] ManuProductReceiptOrderPagedQueryDto pagedQueryDto)
        {
            return await _manuProductReceiptOrderService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 分页查询列表（工单完工入库明细）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ProductReceiptOrderDetail/{id}")]
        public async Task<IEnumerable<ManuProductReceiptOrderDetailDto>> QueryPagedListAsync(long id)
        {
            return await _manuProductReceiptOrderService.QueryByWorkIdAsync(id);
        }
    }
}