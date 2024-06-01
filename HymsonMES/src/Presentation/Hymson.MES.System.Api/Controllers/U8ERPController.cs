using Hymson.MES.SystemServices.Dtos;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// U8 ERP
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
        /// 物料信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateMaterial")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收物料信息", BusinessType.INSERT)]
        public async Task SaveMaterialAsync(ProcMaterialDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveMaterialAsync(requestDtos);
        }

        /// <summary>
        /// 物料组信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateMaterialGroup")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收物料组信息", BusinessType.INSERT)]
        public async Task SaveMaterialGroupAsync(ProcMaterialGroupDto[] requestDtos)
        {
            await Task.CompletedTask;
            //await _kingdeeERPService.SaveMaterialGroupAsync(requestDtos);
        }

        /// <summary>
        /// 供应商信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateSupplier")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收供应商信息", BusinessType.INSERT)]
        public async Task SaveSupplierAsync(WhSupplierDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveSupplierAsync(requestDtos);
        }

        /// <summary>
        /// 客户信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateCustomer")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收客户信息", BusinessType.INSERT)]
        public async Task SaveCustomerAsync(InteCustomDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveSupplierAsync(requestDtos);
        }

        /// <summary>
        /// 仓库信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateWareHouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收仓库信息", BusinessType.INSERT)]
        public async Task SaveWareHouseAsync(WhWarehouseDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveWareHouseAsync(requestDtos);
        }

        /// <summary>
        /// 计量单位信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateUnit")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收计量单位信息", BusinessType.INSERT)]
        public async Task SaveUnitAsync(InteUnitDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveUnitAsync(requestDtos);
        }

        /// <summary>
        /// 班次信息
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateClass")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收班次信息", BusinessType.INSERT)]
        public async Task SaveClassAsync(InteClassDto[] requestDtos)
        {
            await Task.CompletedTask;
            //  await _kingdeeERPService.SaveClassAsync(requestDtos);
        }

        /// <summary>
        /// 工作中心信息（车间/产线）
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>

        [HttpPost("SaveOrUpdateWorkCenter")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收工作中心信息", BusinessType.INSERT)]
        public async Task SaveWorkCenterAsync(InteWorkCenterDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveWorkCenterAsync(requestDtos);
        }

        /// <summary>
        /// 工单及BOM（工单下达时同步)
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        [HttpPost("CreateWorkOrder")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收工单及BOM信息", BusinessType.INSERT)]
        public async Task SaveWorkOrderAsync(WorkOrderDto[] requestDtos)
        {
            await Task.CompletedTask;
            // await _kingdeeERPService.SaveWorkOrderAsync(requestDtos);
        }

        /// <summary>
        /// 生产领料(人工在ERP领料单完成后)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("PickMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产领料信息", BusinessType.INSERT)]
        public async Task SavePickMaterialsAsync(ProductionPickDto requestDto)
        {
            await Task.CompletedTask;
            // await _manuRequistionOrderService.SavePickMaterialsAsync(requestDto);
        }

        /// <summary>
        /// 退料单(人工在ERP退料单完成后)
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("ReturnMaterials")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产退料信息", BusinessType.INSERT)]
        public async Task SaveReturnMaterialsAsync(ProductionReturnMaterialDto requestDto)
        {
            await Task.CompletedTask;
            // await _manuReturnMaterialService.SaveReturnMaterialsAsync(requestDto);
        }

        /// <summary>
        /// 生产入库单（人工在ERP入库单）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("PackInWarehouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产入库单信息", BusinessType.INSERT)]
        public async Task SavePackInWarehouseAsync(PackProductionInWarehouseDto requestDto)
        {
            await Task.CompletedTask;
            // await _productInstocksService.SavePackInWarehouseAsync(requestDto);
        }

        /// <summary>
        /// 生产入库单（人工在ERP入库单）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("InWarehouse")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("接收生产入库单信息", BusinessType.INSERT)]
        public async Task SaveProductionInWarehouseAsync(ProductionInWarehouseDto requestDto)
        {
            await Task.CompletedTask;
            // await _productInstocksService.SaveProductionInWarehouseAsync(requestDto);
        }

        /// <summary>
        /// 获取包装信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [HttpGet("GetPacking/{barCode}")]
        [ProducesResponseType(typeof(ResultDto<IEnumerable<PackingDto>>), 200)]
        // [LogDescription("获取包装信息", BusinessType.OTHER)]
        public async Task<IEnumerable<PackingDto>> GetPackageInfoAsync(string barCode)
        {
            return await Task.FromResult<IEnumerable<PackingDto>>(new List<PackingDto> { });
            //return await _kingdeeERPService.GetPackageInfoAsync(new PackingReqDto
            //{
            //    BarCode = barCode
            //});
        }

    }
}