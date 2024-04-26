using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;
using Hymson.MES.Services.Services.WhWarehouseShelf;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.WhWarehouseShelf
{
    /// <summary>
    /// 控制器（货架）
    /// @author zsj
    /// @date 2023-11-30 07:52:12
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhWarehouseShelfController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<WhWarehouseShelfController> _logger;
        /// <summary>
        /// 服务接口（货架）
        /// </summary>
        private readonly IWhWarehouseShelfService _whWarehouseShelfService;


        /// <summary>
        /// 构造函数（货架）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="whWarehouseShelfService"></param>
        public WhWarehouseShelfController(ILogger<WhWarehouseShelfController> logger, IWhWarehouseShelfService whWarehouseShelfService)
        {
            _logger = logger;
            _whWarehouseShelfService = whWarehouseShelfService;
        }

        /// <summary>
        /// 添加（货架）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("货架", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] WhWarehouseShelfSaveDto saveDto)
        {
             await _whWarehouseShelfService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（货架）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("货架", BusinessType.UPDATE)]
        public async Task UpdateAsync([FromBody] WhWarehouseShelfModifyDto modifyDto)
        {
             await _whWarehouseShelfService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（货架）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("货架", BusinessType.DELETE)]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _whWarehouseShelfService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（货架）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhWarehouseShelfDto?> QueryByIdAsync(long id)
        {
            return await _whWarehouseShelfService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（货架）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhWarehouseShelfDto>> QueryPagedListAsync([FromQuery] WhWarehouseShelfPagedQueryDto pagedQueryDto)
        {
            return await _whWarehouseShelfService.GetPagedListAsync(pagedQueryDto);
        }

    }
}