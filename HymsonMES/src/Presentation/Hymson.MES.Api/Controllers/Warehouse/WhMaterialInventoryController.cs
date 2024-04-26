using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（物料库存）
    /// </summary>
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
        /// <param name="logger"></param>
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
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhMaterialInventoryPageListViewDto>> QueryPagedWhMaterialInventoryAsync([FromQuery] WhMaterialInventoryPagedQueryDto parm)
        {
            return await _whMaterialInventoryService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询是否已存在物料条码
        /// </summary>
        /// <param name="materialBarCode"></param>
        /// <returns></returns>
        [HttpGet("getMaterialBarCodeAny/{materialBarCode}")]
        public async Task<bool> GetMaterialBarCodeAnyAsync(string materialBarCode)
        {
            return await _whMaterialInventoryService.CheckMaterialBarCodeAnyAsync(materialBarCode);
        }

        /// <summary>
        /// 根据物料条码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("barCode/{barCode}")]
        public async Task<WhMaterialInventoryDto?> QueryWhMaterialInventoryByBarCodeAsync(string barCode)
        {
            return await _whMaterialInventoryService.QueryWhMaterialInventoryByBarCodeAsync(barCode);
        }

        /// <summary>
        /// 添加（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("物料库存", BusinessType.INSERT)]
        public async Task AddWhMaterialInventoryAsync([FromBody] WhMaterialInventoryCreateDto parm)
        {
            await _whMaterialInventoryService.CreateWhMaterialInventoryAsync(parm);
        }

        /// <summary>
        /// 批量添加（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("createList")]
        [LogDescription("物料库存", BusinessType.INSERT)]
        [PermissionDescription("wh:materialInventory:insert")]
        public async Task AddWhMaterialInventoryListAsync([FromBody] IEnumerable<WhMaterialInventoryListCreateDto> parm)
        {
            await _whMaterialInventoryService.CreateWhMaterialInventoryListAsync(parm);
        }

        /// <summary>
        /// 更新（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("物料库存", BusinessType.UPDATE)]
        [PermissionDescription("wh:materialInventory:update")]
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
        [LogDescription("物料库存", BusinessType.DELETE)]
        [PermissionDescription("wh:materialInventory:delete")]
        public async Task DeleteWhMaterialInventoryAsync(string ids)
        {
            await _whMaterialInventoryService.DeletesWhMaterialInventoryAsync(ids);
        }
        /// <summary>
        /// 查询物料与供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("materialAndSupplier/{id}")]
        public async Task<ProcMaterialInfoViewDto> GetMaterialAndSupplierByMateialCodeIdAsync(long id)
        {
            return await _whMaterialInventoryService.GetMaterialAndSupplierByMateialCodeIdAsync(id);
        }


        /// <summary>
        /// 来源外部的数据分页查询列表（物料库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("outsidePagelist")]
        public async Task<PagedInfo<WhMaterialInventoryPageListViewDto>> QueryOutsidePagedWhMaterialInventoryAsync([FromQuery] WhMaterialInventoryPagedQueryDto parm)
        {
            return await _whMaterialInventoryService.GetOutsidePageListAsync(parm);
        }

        /// <summary>
        /// 根据物料条码查询外部的
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("outsideBarCode/{barCode}")]
        public async Task<WhMaterialInventoryDetailDto?> QueryOutsideWhMaterialInventoryByBarCodeAsync(string barCode)
        {
            return await _whMaterialInventoryService.QueryOutsideWhMaterialInventoryByBarCodeAsync(barCode);
        }

        /// <summary>
        /// 查询库存物料信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("queryWhMaterialInventoryById/{id}")]
        public async Task<WhMaterialInventoryDetailDto> QueryWhMaterialInventoryByIdAsync(long id)
        {
            return await _whMaterialInventoryService.QueryWhMaterialInventoryDetailByIdAsync(id);
        }

        /// <summary>
        /// 修改外部来源库存
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateOutsideWhMaterialInventory")]
        [LogDescription("修改外部来源库存", BusinessType.UPDATE)]
        //[PermissionDescription("wh:materialInventory:updateOutsideWhMaterialInventory")]
        public async Task UpdateOutsideWhMaterialInventoryAsync(OutsideWhMaterialInventoryModifyDto modifyDto) 
        {
            await _whMaterialInventoryService.UpdateOutsideWhMaterialInventoryAsync(modifyDto);
        }
    }
}