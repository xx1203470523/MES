using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;
using Hymson.MES.Services.Services.WhWarehouseLocation;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WhWarehouseLocation
{
    /// <summary>
    /// 控制器（库位）
    /// @author zsj
    /// @date 2023-11-30 07:52:28
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhWarehouseLocationController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhWarehouseLocationController> _logger;
        /// <summary>
        /// 服务接口（库位）
        /// </summary>
        private readonly IWhWarehouseLocationService _whWarehouseLocationService;


        /// <summary>
        /// 构造函数（库位）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whWarehouseLocationService"></param>
        public WhWarehouseLocationController(ILogger<WhWarehouseLocationController> logger, IWhWarehouseLocationService whWarehouseLocationService)
        {
            _logger = logger;
            _whWarehouseLocationService = whWarehouseLocationService;
        }

        /// <summary>
        /// 添加（库位）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("库位", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] WhWarehouseLocationSaveDto saveDto)
        {
             await _whWarehouseLocationService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（库位）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("库位", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] WhWarehouseLocationModifyDto modifyDto)
        {
             await _whWarehouseLocationService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（库位）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("库位", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _whWarehouseLocationService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（库位）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhWarehouseLocationDto?> QueryByIdAsync(long id)
        {
            return await _whWarehouseLocationService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（库位）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhWarehouseLocationDto>> QueryPagedListAsync([FromQuery] WhWarehouseLocationPagedQueryDto pagedQueryDto)
        {
            return await _whWarehouseLocationService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询列表（库位）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getlist")]
        public async Task<IEnumerable<WhWarehouseLocationDto>> QueryListAsync([FromQuery] WhWarehouseLocationQueryDto queryDto)
        {
            return await _whWarehouseLocationService.GetListAsync(queryDto);
        }

    }
}