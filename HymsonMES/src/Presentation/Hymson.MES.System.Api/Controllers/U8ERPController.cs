using Hymson.MES.SystemServices.Dtos;

using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 对接U8ERP
    /// @author zhaoqing
    /// @date 2023-06-08
    /// </summary>
    [ApiController]
    [Route("SystemService/api/v1/[controller]")]
    [Authorize]
    public class U8ERPController : ControllerBase
    {
        ///// <summary>
        ///// 业务接口（金蝶ERP服务）
        ///// </summary>
        //private readonly IKingdeeERPService _kingdeeERPService;

        ///// <summary>
        ///// 业务接口（生产领料）
        ///// </summary>
        //private readonly IManuRequistionOrderService _manuRequistionOrderService;

        ///// <summary>
        ///// 业务接口（退料）
        ///// </summary>
        //private readonly IManuReturnMaterialService _manuReturnMaterialService;

        ///// <summary>
        ///// 业务接口（入库单）
        ///// </summary>
        //private readonly IWhProductInstocksService _productInstocksService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="kingdeeERPService"></param>
        /// <param name="manuRequistionOrderService"></param>
        /// <param name="manuReturnMaterialService"></param>
        /// <param name="productInstocksService"></param>
        //public U8ERPController(IKingdeeERPService kingdeeERPService,
        //    IManuRequistionOrderService manuRequistionOrderService,
        //    IManuReturnMaterialService manuReturnMaterialService,
        //    IWhProductInstocksService productInstocksService)
        //{
        //    _kingdeeERPService = kingdeeERPService;
        //    _manuRequistionOrderService = manuRequistionOrderService;
        //    _manuReturnMaterialService = manuReturnMaterialService;
        //    _productInstocksService = productInstocksService;
        //}
        public U8ERPController()
        {

        }

        /// <summary>
        /// 新增或者修改物料组信息
        /// </summary>
        /// <param name="groupDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateMaterialGroup")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收物料组信息", BusinessType.INSERT)]
        public async Task SaveMaterialGroupAsync(ProcMaterialGroupDto[] groupDtos)
        {
            //await _kingdeeERPService.SaveMaterialGroupAsync(groupDtos);
        }

        /// <summary>
        /// 新增或者修改物料信息
        /// </summary>
        /// <param name="materialDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateMaterial")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收物料信息", BusinessType.INSERT)]
        public async Task SaveMaterialAsync(ProcMaterialDto[] materialDtos)
        {
           // await _kingdeeERPService.SaveMaterialAsync(materialDtos);
        }

        /// <summary>
        /// 新增或者修改供应商信息
        /// </summary>
        /// <param name="supplierDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateSupplier")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收供应商信息", BusinessType.INSERT)]
        public async Task SaveSupplierAsync(WhSupplierDto[] supplierDtos)
        {
           // await _kingdeeERPService.SaveSupplierAsync(supplierDtos);
        }

        /// <summary>
        /// 新增或者修改仓库信息
        /// </summary>
        /// <param name="warehouseDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateWareHouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收仓库信息", BusinessType.INSERT)]
        public async Task SaveWareHouseAsync(WhWarehouseDto[] warehouseDtos)
        {
           // await _kingdeeERPService.SaveWareHouseAsync(warehouseDtos);
        }

        /// <summary>
        /// 新增或者修改计量单位信息
        /// </summary>
        /// <param name="inteUnitDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateUnit")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收计量单位信息", BusinessType.INSERT)]
        public async Task SaveUnitAsync(InteUnitDto[] inteUnitDtos)
        {
           // await _kingdeeERPService.SaveUnitAsync(inteUnitDtos);
        }

        /// <summary>
        /// 新增或者修改班次信息
        /// </summary>
        /// <param name="inteClassDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateClass")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收班次信息", BusinessType.INSERT)]
        public async Task SaveClassAsync(InteClassDto[] inteClassDtos)
        {
          //  await _kingdeeERPService.SaveClassAsync(inteClassDtos);
        }

        /// <summary>
        /// 新增或者修改工作中心信息
        /// </summary>
        /// <param name="workCenterDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOrUpdateWorkCenter")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收工作中心信息", BusinessType.INSERT)]
        public async Task SaveWorkCenterAsync(InteWorkCenterDto[] workCenterDtos)
        {
           // await _kingdeeERPService.SaveWorkCenterAsync(workCenterDtos);
        }

        /// <summary>
        /// 工单及BOM（工单下达时同步)
        /// </summary>
        /// <param name="workOrderDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateWorkOrder")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收工单及BOM信息", BusinessType.INSERT)]
        public async Task SaveWorkOrderAsync(WorkOrderDto[] workOrderDtos)
        {
           // await _kingdeeERPService.SaveWorkOrderAsync(workOrderDtos);
        }

        /// <summary>
        /// 生产领料(人工在ERP领料单完成后)
        /// </summary>
        /// <param name="productionPickDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PickMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产领料信息", BusinessType.INSERT)]
        public async Task SavePickMaterialsAsync(ProductionPickDto productionPickDto)
        {
           // await _manuRequistionOrderService.SavePickMaterialsAsync(productionPickDto);
        }

        /// <summary>
        /// 退料单(人工在ERP退料单完成后)
        /// </summary>
        /// <param name="returnMaterialDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ReturnMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产退料信息", BusinessType.INSERT)]
        public async Task SaveReturnMaterialsAsync(ProductionReturnMaterialDto returnMaterialDto)
        {
           // await _manuReturnMaterialService.SaveReturnMaterialsAsync(returnMaterialDto);
        }

        /// <summary>
        /// 获取包装信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPaking/{barCode}")]
        [ProducesResponseType(typeof(ResultDto<List<PackingDto>>), 200)]
        // [LogDescription("获取包装信息", BusinessType.OTHER)]
        public async Task<List<PackingDto>> GetPackageInfoAsync(string barCode)
        {
            return new List<PackingDto>();
            //return await _kingdeeERPService.GetPackageInfoAsync(new PackingReqDto
            //{
            //    BarCode = barCode
            //});
        }

        /// <summary>
        /// 生产入库单(人工在ERP入库单)
        /// </summary>
        /// <param name="inWarehouseDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PackInWarehouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产入库单信息", BusinessType.INSERT)]
        public async Task SavePackInWarehouseAsync(PackProductionInWarehouseDto inWarehouseDto)
        {
           // await _productInstocksService.SavePackInWarehouseAsync(inWarehouseDto);
        }

        /// <summary>
        /// 生产入库单(人工在ERP入库单)
        /// </summary>
        /// <param name="inWarehouseDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InWarehouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产入库单信息", BusinessType.INSERT)]
        public async Task SaveProductionInWarehouseAsync(ProductionInWarehouseDto inWarehouseDto)
        {
           // await _productInstocksService.SaveProductionInWarehouseAsync(inWarehouseDto);
        }
    }
}