using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhShipment;
using Hymson.MES.Services.Services.WhShipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WhShipment
{
    /// <summary>
    /// 控制器（出货单）
    /// @author Jam
    /// @date 2024-03-04 04:16:33
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhShipmentController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhShipmentController> _logger;
        /// <summary>
        /// 服务接口（出货单）
        /// </summary>
        private readonly IWhShipmentService _whShipmentService;


        /// <summary>
        /// 构造函数（出货单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whShipmentService"></param>
        public WhShipmentController(ILogger<WhShipmentController> logger, IWhShipmentService whShipmentService)
        {
            _logger = logger;
            _whShipmentService = whShipmentService;
        }

        /// <summary>
        /// 添加（出货单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] WhShipmentSaveDto saveDto)
        {
             await _whShipmentService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（出货单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] WhShipmentSaveDto saveDto)
        {
             await _whShipmentService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（出货单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _whShipmentService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（出货单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhShipmentDto?> QueryByIdAsync(long id)
        {
            return await _whShipmentService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（出货单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhShipmentDto>> QueryPagedListAsync([FromQuery] WhShipmentPagedQueryDto pagedQueryDto)
        {
            return await _whShipmentService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 分页查询列表（OQC）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelistToOQC")]
        public async Task<PagedInfo<WhShipmentDto>> QueryPagedListToOQCAsync([FromQuery] WhShipmentPagedQueryDto pagedQueryDto)
        {
            return await _whShipmentService.GetPagedListToOQCAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询列表（出货单详情，物料，客户）
        /// </summary>
        /// <param name="whShipmentQueryDto"></param>
        /// <returns></returns>
        [HttpGet("QueryShipmentSupplierMaterial")]
        public async Task<IEnumerable<WhShipmentSupplierMaterialViewDto>> QueryShipmentSupplierMaterialAsync([FromQuery] WhShipmentQueryDto whShipmentQueryDto)
        {
            return await _whShipmentService.QueryShipmentSupplierMaterialAsync(whShipmentQueryDto);
        }
    }
}