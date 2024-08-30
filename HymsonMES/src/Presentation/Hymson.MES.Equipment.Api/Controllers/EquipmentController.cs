using Hymson.MES.BackgroundServices.NIO.Services;
using Hymson.MES.BackgroundServices.NIO.Services.ERP;
using Hymson.MES.BackgroundServices.Rotor.Services;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos;

using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.EquipmentCollect;
using Hymson.MES.EquipmentServices.Services.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny.Common;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（设备）
    /// </summary>
    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1")]
    public partial class EquipmentController : ControllerBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<EquipmentController> _logger;
        private readonly IEquipmentCollectService _equipmentService;

        /// <summary>
        /// 生产服务接口
        /// </summary>
        private readonly IManufactureService _manufactureService;

        /// <summary>
        /// 条码绑定
        /// </summary>
        private readonly ISfcBindingService _sfcBindingService;
        private readonly IEquCommonService _equCommonService;

        /// <summary>
        /// 马威转子
        /// </summary>
        private readonly IManuInOutBoundService _mavel;

        /// <summary>
        /// 
        /// </summary>
        private readonly IErpDataPushService _erpDataPushService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 主数据推送
        /// </summary>
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 业务数据
        /// </summary>
        private readonly IBuzDataPushService _buzDataPushService;

        /// <summary>
        /// 异常数据处理
        /// </summary>
        private readonly IAbnormalDataService _abnormalDataService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manufactureService"></param>
        /// <param name="equCommonService"></param>
        /// <param name="equipmentCollectService"></param>
        /// <param name="sfcBindingService"></param>
        /// <param name="manuInOutBoundService"></param>
        public EquipmentController(ILogger<EquipmentController> logger,
            IManufactureService manufactureService,
            IEquCommonService equCommonService,
            IEquipmentCollectService equipmentCollectService,
            ISfcBindingService sfcBindingService,
            IManuInOutBoundService manuInOutBoundService,
            IErpDataPushService erpDataPushService,
            IPushNIOService pushNIOService,
            IMasterDataPushService masterDataPushService,
            IBuzDataPushService buzDataPushService,
            IAbnormalDataService abnormalDataService)
        {
            _logger = logger;
            _manufactureService = manufactureService;
            _sfcBindingService = sfcBindingService;
            _equipmentService = equipmentCollectService;
            _equCommonService = equCommonService;
            _mavel = manuInOutBoundService;
            _erpDataPushService = erpDataPushService;
            _pushNIOService = pushNIOService;
            _masterDataPushService = masterDataPushService;
            _buzDataPushService = buzDataPushService;
            _abnormalDataService = abnormalDataService;
        }


        /// <summary>
        /// 马威测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("mavel")]
        [AllowAnonymous]
        public async Task TestAsync()
        {
            //DateTime beginDate = DateTime.Parse("2024-05-17 11:49:09.340");
            //DateTime endDate = DateTime.Parse("2024-05-22 13:08:20.273");

            ////主数据（控制项）
            //await _masterDataPushService.FieldAsync();
            ////主数据（工站）
            //await _masterDataPushService.StationAsync();
            ////主数据（产品）
            //await _masterDataPushService.ProductAsync();
            ////主数据（一次合格率目标）
            //await _masterDataPushService.PassrateTargetAsync();

            ////业务数据（控制项）
            //await _buzDataPushService.CollectionAsync();
            ////业务数据（生产业务）
            //await _buzDataPushService.ProductionAsync();
            //业务数据（材料清单）
            await _buzDataPushService.MaterialAsync();
            ////业务数据（产品一次合格率）
            //await _buzDataPushService.PassrateProductAsync();
            ////业务数据（缺陷业务
            //await _buzDataPushService.IssueAsync();
            ////业务数据（工单业务）
            //await _buzDataPushService.WorkOrderAsync();

            ////NIO合作伙伴精益与库存信息
            //await _erpDataPushService.NioStockInfoAsync();
            ////关键下级键
            //await _erpDataPushService.NioKeyItemInfoAsync();
            ////实际交付情况
            //await _erpDataPushService.NioActualDeliveryAsync();

            //推送数据
            //await _pushNIOService.ExecutePushAsync();

            //await _pushNIOService.ExecutePushFailAsync();

            //await _mavel.InOutBoundAsync(100);

            //await _abnormalDataService.RepeatParamAsync(7);
        }

        /// <summary>
        /// 马威测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("mavel002")]
        [AllowAnonymous]
        public List<long> Test002Async()
        {
            List<long> resultList = new List<long>();
            DateTime date = HymsonClock.Now();

            //date = date.AddHours(8);
            long time1 = (long)((DateTime)date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
            resultList.Add(time1);

            // 首先将本地时间转换为UTC时间  
            DateTime utcDateTime = ((DateTime)date).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = utcDateTime - epoch;
            long tiem2 = (long)timeSpan.TotalSeconds;
            resultList.Add(tiem2);


            return resultList;
        }

        /// <summary>
        /// 设备心跳
        /// HY-MES-EQU-002
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Heartbeat")]
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatDto request)
        {
            _logger.LogDebug(request.ToSerialize());


            await _equipmentService.EquipmentHeartbeatAsync(request);
        }

        /// <summary>
        /// 设备状态监控
        /// HY-MES-EQU-003
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("State")]
        public async Task EquipmentStateAsync(EquipmentStateDto request)
        {
            _logger.LogDebug(request.ToSerialize());


            await _equipmentService.EquipmentStateAsync(request);
        }

        /// <summary>
        /// 设备报警
        /// HY-MES-EQU-004
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Alarm")]
        public async Task EquipmentAlarmAsync(EquipmentAlarmDto request)
        {
            _logger.LogDebug(request.ToSerialize());
            // await Task.CompletedTask;

            await _equipmentService.EquipmentAlarmAsync(request);
        }

        /// <summary>
        /// 设备停机原因
        /// HY-MES-EQU-005
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DownReason")]
        public async Task EquipmentDownReasonAsync(EquipmentDownReasonDto request)
        {
            _logger.LogDebug(request.ToSerialize());
            //await Task.CompletedTask;

            await _equipmentService.EquipmentDownReasonAsync(request);
        }


        /// <summary>
        /// 创建条码（半成品）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBarCode")]
        [LogDescription("创建条码", BusinessType.OTHER, "CreateBarCode", ReceiverTypeEnum.MES)]
        public async Task<IEnumerable<string>> CreateBarCodeBySemiProductAsync(BaseDto dto)
        {
            return await _manufactureService.CreateBarcodeBySemiProductIdAsync(dto);
        }

        /// <summary>
        /// 创建条码（电芯）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateCellBarCode")]
        [LogDescription("创建电芯条码", BusinessType.OTHER, "CreateCellBarCode", ReceiverTypeEnum.MES)]
        public async Task<IEnumerable<string>> CreateCellBarCodeAsync(BaseDto dto)
        {
            return await _manufactureService.CreateCellBarCodeAsync(dto);
        }

        /// <summary>
        ///条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("SfcBinding")]
        [LogDescription("条码绑定", BusinessType.OTHER, "SFCBinding", ReceiverTypeEnum.MES)]
        public async Task SfcBindingAsync(SfcBindingDto sfcBindingDto)
        {
            await _sfcBindingService.SfcCirculationBindAsync(sfcBindingDto);
        }

        /// <summary>
        /// 进站 HY-MES-EQU-015
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStation")]
        [LogDescription("进站", BusinessType.OTHER, "InStation", ReceiverTypeEnum.MES)]
        public async Task InBoundAsync(InBoundDto request)
        {
            await _manufactureService.InBoundAsync(request);
        }

        /// <summary>
        /// 进站（多个）HY-MES-EQU-016
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStationMore")]
        [LogDescription("多个进站", BusinessType.OTHER, "InStationMore", ReceiverTypeEnum.MES)]
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _manufactureService.InBoundMoreAsync(request);
        }

        /// <summary>
        /// 出站 HY-MES-EQU-017
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStation")]
        [LogDescription("出站", BusinessType.OTHER, "OutStation", ReceiverTypeEnum.MES)]
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _manufactureService.OutBoundAsync(request);
        }

        /// <summary>
        /// 出站（多个） HY-MES-EQU-018
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStationMore")]
        [LogDescription("多个出站", BusinessType.OTHER, "OutStationMore", ReceiverTypeEnum.MES)]
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _manufactureService.OutBoundMoreAsync(request);
        }

        /// <summary>
        /// 载具进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBoundCarrier")]
        [LogDescription("载具进站", BusinessType.OTHER, "InBoundCarrier", ReceiverTypeEnum.MES)]
        public async Task InBoundCarrierAsync(InBoundCarrierDto request)
        {
            await _manufactureService.InBoundCarrierAsync(request);
        }

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBoundCarrier")]
        [LogDescription("载具出站", BusinessType.OTHER, "OutBoundCarrier", ReceiverTypeEnum.MES)]
        public async Task OutBoundCarrierAsync(OutBoundCarrierDto request)
        {
            await _manufactureService.OutBoundCarrierAsync(request);
        }

        /// <summary>
        /// 生成国标码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CreateGBCode")]
        [LogDescription("生成国标码", BusinessType.OTHER, "CreateGBCode", ReceiverTypeEnum.MES)]
        public async Task<string> CreateGBCodeAsync(ManuMergeRequestDto request)
        {
            return await _manufactureService.MergeAsync(request);
        }

        /// <summary>
        /// CCD文件上传完成006
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdFileUploadComplete")]
        [LogDescription("CCD文件上传完成006", BusinessType.OTHER, "006", ReceiverTypeEnum.MES)]
        public async Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto)
        {
            await _equCommonService.CcdFileUploadCompleteAsync(dto);
            //TODO
            //1. 新增表 ccd_file_upload_complete_record，用于记录每个条码对应的CCD文件路径及是否合格
            //  明细和主表记录到一起
        }

    }
}
