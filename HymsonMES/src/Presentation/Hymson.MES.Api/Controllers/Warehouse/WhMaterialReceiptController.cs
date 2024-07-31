using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WHMaterialReceipt;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;
using Hymson.MES.Services.Services.WHMaterialReceipt;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WHMaterialReceipt
{
    /// <summary>
    /// 控制器（物料收货表）
    /// @author Jam
    /// @date 2024-03-04 02:20:06
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhMaterialReceiptController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhMaterialReceiptController> _logger;
        /// <summary>
        /// 服务接口（物料收货表）
        /// </summary>
        private readonly IWhMaterialReceiptService _whMaterialReceiptService;


        /// <summary>
        /// 构造函数（物料收货表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whMaterialReceiptService"></param>
        public WhMaterialReceiptController(ILogger<WhMaterialReceiptController> logger, IWhMaterialReceiptService whMaterialReceiptService)
        {
            _logger = logger;
            _whMaterialReceiptService = whMaterialReceiptService;
        }

        /// <summary>
        /// 添加（物料收货表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [LogDescription("物料收货表", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] WhMaterialReceiptSaveDto saveDto)
        {
            await _whMaterialReceiptService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（物料收货表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [LogDescription("物料收货表", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] WhMaterialReceiptSaveDto saveDto)
        {
            await _whMaterialReceiptService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（物料收货表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        [LogDescription("物料收货表", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _whMaterialReceiptService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（物料收货表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhMaterialReceiptDto?> QueryByIdAsync(long id)
        {
            return await _whMaterialReceiptService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（物料收货表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("pagelist")]
        public async Task<PagedInfo<WhMaterialReceiptDto>> QueryPagedListAsync([FromQuery] WhMaterialReceiptPagedQueryDto pagedQueryDto)
        {
            return await _whMaterialReceiptService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（物料收货表）
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        [HttpGet("details/{receiptId}")]
        public async Task<IEnumerable<ReceiptMaterialDetailDto>> QueryDetailByReceiptIdAsync(long receiptId)
        {
            return await _whMaterialReceiptService.QueryDetailByReceiptIdAsync(receiptId);
        }

    }
}