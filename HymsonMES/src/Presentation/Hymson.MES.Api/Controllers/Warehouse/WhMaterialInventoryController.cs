/*
 *creator: Karl
 *
 *describe: 物料库存    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Warehouse;
//using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（物料库存）
    /// @author pengxin
    /// @date 2023-03-06 03:27:59
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhMaterialInventoryController : ControllerBase
    {
        /// <summary>
        /// 接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryService _whMaterialInventoryService;
        private readonly ILogger<WhMaterialInventoryController> _logger;

        /// <summary>
        /// 构造函数（物料库存）
        /// </summary>
        /// <param name="whMaterialInventoryService"></param>
        public WhMaterialInventoryController(IWhMaterialInventoryService whMaterialInventoryService, ILogger<WhMaterialInventoryController> logger)
        {
            _whMaterialInventoryService = whMaterialInventoryService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<WhMaterialInventoryDto>> QueryPagedWhMaterialInventoryAsync([FromQuery] WhMaterialInventoryPagedQueryDto parm)
        {
            return await _whMaterialInventoryService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（物料库存）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhMaterialInventoryDto> QueryWhMaterialInventoryByIdAsync(long id)
        {
            return await _whMaterialInventoryService.QueryWhMaterialInventoryByIdAsync(id);
        }

        /// <summary>
        /// 添加（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddWhMaterialInventoryAsync([FromBody] WhMaterialInventoryCreateDto parm)
        {
             await _whMaterialInventoryService.CreateWhMaterialInventoryAsync(parm);
        }

        /// <summary>
        /// 更新（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateWhMaterialInventoryAsync([FromBody] WhMaterialInventoryModifyDto parm)
        {
             await _whMaterialInventoryService.ModifyWhMaterialInventoryAsync(parm);
        }

        /// <summary>
        /// 删除（物料库存）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteWhMaterialInventoryAsync(string ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _whMaterialInventoryService.DeletesWhMaterialInventoryAsync(ids);
        }

    }
}