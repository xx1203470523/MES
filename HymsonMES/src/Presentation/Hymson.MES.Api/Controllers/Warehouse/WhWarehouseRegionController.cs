using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.MES.Services.Services.WhWarehouseRegion;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WhWarehouseRegion
{
    /// <summary>
    /// 控制器（库区）
    /// @author zsj
    /// @date 2023-11-30 04:17:35
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhWarehouseRegionController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhWarehouseRegionController> _logger;
        /// <summary>
        /// 服务接口（库区）
        /// </summary>
        private readonly IWhWarehouseRegionService _whWarehouseRegionService;


        /// <summary>
        /// 构造函数（库区）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whWarehouseRegionService"></param>
        public WhWarehouseRegionController(ILogger<WhWarehouseRegionController> logger, IWhWarehouseRegionService whWarehouseRegionService)
        {
            _logger = logger;
            _whWarehouseRegionService = whWarehouseRegionService;
        }

        /// <summary>
        /// 添加（库区）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("库区", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] WhWarehouseRegionSaveDto saveDto)
        {
             await _whWarehouseRegionService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（库区）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("库区", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] WhWarehouseRegionModifyDto modifyDto)
        {
             await _whWarehouseRegionService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（库区）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("库区", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _whWarehouseRegionService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（库区）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhWarehouseRegionDto?> QueryByIdAsync(long id)
        {
            return await _whWarehouseRegionService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（库区）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhWarehouseRegionDto>> QueryPagedListAsync([FromQuery] WhWarehouseRegionPagedQueryDto pagedQueryDto)
        {
            return await _whWarehouseRegionService.GetPagedListAsync(pagedQueryDto);
        }

    }
}