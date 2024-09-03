using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.MES.Services.Services.NIO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.NIO
{
    /// <summary>
    /// 控制器（物料发货信息表）
    /// @author YXX
    /// @date 2024-09-03 10:03:18
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NioPushActualDeliveryController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<NioPushActualDeliveryController> _logger;
        /// <summary>
        /// 服务接口（物料发货信息表）
        /// </summary>
        private readonly INioPushActualDeliveryService _nioPushActualDeliveryService;


        /// <summary>
        /// 构造函数（物料发货信息表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushActualDeliveryService"></param>
        public NioPushActualDeliveryController(ILogger<NioPushActualDeliveryController> logger, INioPushActualDeliveryService nioPushActualDeliveryService)
        {
            _logger = logger;
            _nioPushActualDeliveryService = nioPushActualDeliveryService;
        }

        /// <summary>
        /// 添加（物料发货信息表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] NioPushActualDeliverySaveDto saveDto)
        {
             await _nioPushActualDeliveryService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（物料发货信息表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] NioPushActualDeliverySaveDto saveDto)
        {
             await _nioPushActualDeliveryService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（物料发货信息表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _nioPushActualDeliveryService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（物料发货信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NioPushActualDeliveryDto?> QueryByIdAsync(long id)
        {
            return await _nioPushActualDeliveryService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（物料发货信息表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<NioPushActualDeliveryDto>> QueryPagedListAsync([FromQuery] NioPushActualDeliveryPagedQueryDto pagedQueryDto)
        {
            return await _nioPushActualDeliveryService.GetPagedListAsync(pagedQueryDto);
        }

    }
}