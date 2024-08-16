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
        /// 物料拆分与合并条码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("getbarCode/{barCode}")]
        public async Task<WhMaterialInventoryPageListViewDto?> QueryWhMaterialBarCodeAsync(string barCode)
        {
            return await _whMaterialInventoryService.QueryWhMaterialBarCodeAsync(barCode);
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
        public async Task UpdateOutsideWhMaterialInventoryAsync(OutsideWhMaterialInventoryModifyDto modifyDto) 
        {
            await _whMaterialInventoryService.UpdateOutsideWhMaterialInventoryAsync(modifyDto);
        }

        /// <summary>
        /// 物料条码拆分
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("barcodeSplitAdjust")]
        [LogDescription("条码拆分", BusinessType.INSERT)]
        public async Task<string> BarcodeSplitAdjustAsync(MaterialBarCodeSplitAdjustDto adjustDto)
        {
            return await _whMaterialInventoryService.BarcodeSplitAdjustAsync(adjustDto);
        }

        /// <summary>
        /// 物料合并
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("barcodeMergeAdjust")]
        [LogDescription("物料合并", BusinessType.INSERT)]
        public async Task<string> BarcodeMergeAdjustAsync(MaterialBarCodeMergeAdjust adjustDto)
        {
            return await _whMaterialInventoryService.BarcodeMergeAdjustAsync(adjustDto);
        }
        /// <summary>
        /// 领料申请 按照工单数量领料
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("PickMaterialsRequest")]
        [LogDescription("领料申请", BusinessType.INSERT)]
        public async Task PickMaterialsRequestAsync([FromBody] PickMaterialsRequest request)
        {
            await _whMaterialInventoryService.PickMaterialsRequestAsync(request);
        }
        /// <summary>
        /// 领料申请  按照物料明细领料
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("PickMaterialsRequestV2")]
        [LogDescription("领料申请", BusinessType.INSERT)]
        public async Task PickMaterialsRequestAsync([FromBody] PickMaterialsRequestV2 request)
        {
            await _whMaterialInventoryService.PickMaterialsRequestAsync(request);
        }
        /// <summary>
        /// 取消领料
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("PickMaterialsCancel")]
        [LogDescription("取消领料", BusinessType.INSERT)]
        public async Task<bool> PickMaterialsCancelAsync([FromBody] PickMaterialsCancel request)
        {
            return await _whMaterialInventoryService.PickMaterialsCancelAsync(request);
        }
        /// <summary>
        /// 退料申请
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("MaterialReturnRequest")]
        [LogDescription("退料申请", BusinessType.INSERT)]
        public async Task MaterialReturnRequestAsync([FromBody] MaterialReturnRequest request)
        {
            await _whMaterialInventoryService.MaterialReturnRequestAsync(request);
        }
        /// <summary>
        /// 取消退料
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("MaterialReturnCancel")]
        [LogDescription("取消退料", BusinessType.INSERT)]
        public async Task<bool> MaterialReturnCancelAsync([FromBody] MaterialReturnCancel request)
        {
            return await _whMaterialInventoryService.MaterialReturnCancelAsync(request);
        }

        /// <summary>
        /// 入库申请
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("ProductReceiptRequest")]
        [LogDescription("入库申请", BusinessType.INSERT)]
        public async Task ProductReceiptRequestAsync([FromBody] ProductReceiptRequest request)
        {
            await _whMaterialInventoryService.ProductReceiptRequestAsync(request);
        }
        /// <summary>
        /// 取消入库
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("ProductReceiptCancel")]
        [LogDescription("取消入库", BusinessType.INSERT)]
        public async Task<bool> ProductReceiptCancelAsync([FromBody] MaterialReturnCancel request)
        {
            return await _whMaterialInventoryService.ProductReceiptCancelAsync(request);
        }
      /// <summary>
        /// 根据工单查询工单的领料列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getByOrderId/{id}")]
        public async Task<IEnumerable<WhMaterialInventoryDetailDto>> GetPickMaterialsByOrderidAsync(long id)
        {
            return await _whMaterialInventoryService.GetPickMaterialsByOrderidAsync(id);
        }

        /// <summary>
        /// 副产品入库申请
        /// </summary>
        /// <param name="request"></param>
        [HttpPost("WasteProductReceipt")]
        [LogDescription("副产品入库申请", BusinessType.INSERT)]
        public async Task WasteProductReceiptRequestAsync([FromBody] WasteProductReceiptRequest request)
        {
            await _whMaterialInventoryService.WasteProductReceiptRequestAsync(request);
        }
    }
}