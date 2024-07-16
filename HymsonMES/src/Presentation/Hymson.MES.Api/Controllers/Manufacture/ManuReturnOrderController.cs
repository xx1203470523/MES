using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（生产退料单）
    /// @author wxk
    /// @date 2024-06-22 02:24:51
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuReturnOrderController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuReturnOrderController> _logger;
        /// <summary>
        /// 服务接口（生产退料单）
        /// </summary>
        private readonly IManuReturnOrderService _manuReturnOrderService;


        /// <summary>
        /// 构造函数（生产退料单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuReturnOrderService"></param>
        public ManuReturnOrderController(ILogger<ManuReturnOrderController> logger, IManuReturnOrderService manuReturnOrderService)
        {
            _logger = logger;
            _manuReturnOrderService = manuReturnOrderService;
        }

        /// <summary>
        /// 添加（生产退料单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuReturnOrderSaveDto saveDto)
        {
             await _manuReturnOrderService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（生产退料单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuReturnOrderSaveDto saveDto)
        {
             await _manuReturnOrderService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（生产退料单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuReturnOrderService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（生产退料单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuReturnOrderDto?> QueryByIdAsync(long id)
        {
            return await _manuReturnOrderService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（生产退料单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuReturnOrderDto>> QueryPagedListAsync([FromQuery] ManuReturnOrderPagedQueryDto pagedQueryDto)
        {
            return await _manuReturnOrderService.GetPagedListAsync(pagedQueryDto);
        }

    }
}