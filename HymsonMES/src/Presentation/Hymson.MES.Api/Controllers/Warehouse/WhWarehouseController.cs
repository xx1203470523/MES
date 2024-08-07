using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.WhWareHouse;
using Hymson.MES.Services.Services.WhWareHouse;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WhWareHouse
{
    /// <summary>
    /// 控制器（仓库）
    /// @author zsj
    /// @date 2023-11-28 10:29:43
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhWarehouseController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhWarehouseController> _logger;
        /// <summary>
        /// 服务接口（仓库）
        /// </summary>
        private readonly IWhWarehouseService _whWarehouseService;


        /// <summary>
        /// 构造函数（仓库）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whWarehouseService"></param>
        public WhWarehouseController(ILogger<WhWarehouseController> logger, IWhWarehouseService whWarehouseService)
        {
            _logger = logger;
            _whWarehouseService = whWarehouseService;
        }

        /// <summary>
        /// 添加（仓库）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("仓库", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] WhWarehouseSaveDto saveDto)
        {
             await _whWarehouseService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（仓库）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("仓库", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] WhWarehouseModifyDto modifyDto)
        {
             await _whWarehouseService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（仓库）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("仓库", BusinessType.DELETE)]
        public async Task DeleteAsync(long[] ids)
        {
            await _whWarehouseService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（仓库）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhWarehouseDto?> QueryByIdAsync(long id)
        {
            return await _whWarehouseService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询所有仓库信息（仓库）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("warehouselist")]
        public async Task<IEnumerable<SelectOptionDto>> GetWarehouseListAsync()
        {
            return await _whWarehouseService.GetWarehouseListAsync();
        }

        /// <summary>
        /// 分页查询列表（仓库）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhWarehouseDto>> QueryPagedListAsync([FromQuery] WhWarehousePagedQueryDto pagedQueryDto)
        {
            return await _whWarehouseService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 分页查询列表（仓库）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelistCopy")]
        public async Task<PagedInfo<WhWarehouseDto>> QueryPagedListCopyAsync([FromQuery] WhWarehousePagedQueryDto pagedQueryDto)
        {
            return await _whWarehouseService.GetPagedListCopyAsync(pagedQueryDto);
        }

    }
}