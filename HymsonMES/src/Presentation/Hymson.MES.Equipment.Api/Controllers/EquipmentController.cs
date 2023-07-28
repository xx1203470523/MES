﻿using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.MES.EquipmentServices.Dtos.Feeding;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.NGData;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.MES.EquipmentServices.Dtos.OutPutQty;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.MES.EquipmentServices.Dtos.SingleBarCodeLoadingVerification;
using Hymson.MES.EquipmentServices.Services;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Services.EquipmentCollect;
using Hymson.MES.EquipmentServices.Services.Feeding;
using Hymson.MES.EquipmentServices.Services.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Services.InBound;
using Hymson.MES.EquipmentServices.Services.InboundInContainer;
using Hymson.MES.EquipmentServices.Services.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Services.Manufacture.InStation;
using Hymson.MES.EquipmentServices.Services.OutBound;
using Hymson.MES.EquipmentServices.Services.OutPutQty;
using Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Services.SfcBinding;
using Hymson.MES.EquipmentServices.Services.SfcCirculation;
using Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 设备接口（海目星设备）
    /// </summary>
    [Route("EquipmentService/api/v1/EquApi")]
    [ApiController]
    public class EquipmentController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquipmentController> _logger;
        /// <summary>
        /// 进站
        /// </summary>
        private readonly IInStationService _InStationService;
        /// <summary>
        /// 条码绑定
        /// </summary>
        private readonly ISfcBindingService _sfcBindingService;
        /// <summary>
        /// 业务接口（设备监控服务）
        /// </summary>
        private readonly IEquipmentCollectService _equipmentService;
        private readonly IBindSFCService _bindSFCService;
        private readonly IBindContainerService _bindContainerService;
        private readonly ICCDFileUploadCompleteService _cCDFileUploadCompleteService;
        private readonly IFeedingService _feedingService;
        private readonly IFeedingConsumptionService _feedingConsumptionService;
        private readonly IGenerateModuleSFCService _generateModuleSFCService;
        private readonly IInboundInContainerService _inboundInContainerService;
        private readonly IInboundInSFCContainerService _inboundInSFCContainerService;
        private readonly IOutPutQtyService _outPutQtyService;
        private readonly IQueryContainerBindSfcService _queryContainerBindSfcService;
        private readonly ISingleBarCodeLoadingVerificationService _singleBarCodeLoadingVerificationService;
        private readonly IInBoundService _inBoundService;
        private readonly IOutBoundService _outBoundService;
        private readonly INGDataService _ngDataService;
        private readonly ISfcCirculationService _sfcCirculationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuInStationService"></param>
        /// <param name="sfcBindingService"></param>
        /// <param name="equipmentService"></param>
        /// <param name="bindSFCService"></param>
        /// <param name="bindContainerService"></param>
        /// <param name="cCDFileUploadCompleteService"></param>
        /// <param name="feedingService"></param>
        /// <param name="feedingConsumptionService"></param>
        /// <param name="generateModuleSFCService"></param>
        /// <param name="inboundInContainerService"></param>
        /// <param name="inboundInSFCContainerService"></param>
        /// <param name="outPutQtyService"></param>
        /// <param name="queryContainerBindSfcService"></param>
        /// <param name="singleBarCodeLoadingVerificationService"></param>
        /// <param name="inBoundService"></param>
        /// <param name="outBoundService"></param>
        /// <param name="ngDataService"></param>
        /// <param name="sfcCirculationService"></param>
        public EquipmentController(ILogger<EquipmentController> logger,
            IInStationService manuInStationService,
            ISfcBindingService sfcBindingService,
            IEquipmentCollectService equipmentService,
            IBindSFCService bindSFCService,
            IBindContainerService bindContainerService,
            ICCDFileUploadCompleteService cCDFileUploadCompleteService,
            IFeedingService feedingService,
            IFeedingConsumptionService feedingConsumptionService,
            IGenerateModuleSFCService generateModuleSFCService,
            IInboundInContainerService inboundInContainerService,
            IInboundInSFCContainerService inboundInSFCContainerService,
            IOutPutQtyService outPutQtyService,
            IQueryContainerBindSfcService queryContainerBindSfcService,
            ISingleBarCodeLoadingVerificationService singleBarCodeLoadingVerificationService,
            IInBoundService inBoundService,
            IOutBoundService outBoundService,
            INGDataService ngDataService,
            ISfcCirculationService sfcCirculationService)
        {
            _logger = logger;
            _InStationService = manuInStationService;
            _sfcBindingService = sfcBindingService;
            _equipmentService = equipmentService;
            _bindSFCService = bindSFCService;
            _bindContainerService = bindContainerService;
            _cCDFileUploadCompleteService = cCDFileUploadCompleteService;
            _feedingService = feedingService;
            _feedingConsumptionService = feedingConsumptionService;
            _generateModuleSFCService = generateModuleSFCService;
            _inboundInContainerService = inboundInContainerService;
            _inboundInSFCContainerService = inboundInSFCContainerService;
            _outPutQtyService = outPutQtyService;
            _queryContainerBindSfcService = queryContainerBindSfcService;
            _singleBarCodeLoadingVerificationService = singleBarCodeLoadingVerificationService;
            _inBoundService = inBoundService;
            _outBoundService = outBoundService;
            _ngDataService = ngDataService;
            _sfcCirculationService = sfcCirculationService;
        }

        /// <summary>
        ///进站
        /// </summary>
        /// <param name="inStationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InStation")]
        public async Task InStationAsync(InStationDto inStationDto)
        {
            await _InStationService.InStationExecuteAsync(inStationDto);
        }

        /// <summary>
        ///条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("SfcBinding")]
        public async Task SfcBindingAsync(SfcBindingDto sfcBindingDto)
        {
            await _sfcBindingService.SfcBindingAsync(sfcBindingDto);
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
            await _equipmentService.EquipmentDownReasonAsync(request);
        }

        /// <summary>
        /// 设备过程参数采集（无在制品条码）
        /// HY-MES-EQU-010
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProcessParam")]
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto request)
        {
            await _equipmentService.EquipmentProcessParamAsync(request);
        }

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// HY-MES-EQU-011
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParamInNotCanSFC")]
        public async Task<string> EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCDto request)
        {
            return await _equipmentService.EquipmentProductProcessParamInNotCanSFCAsync(request);
        }

        /// <summary>
        /// 设备产品过程参数采集
        /// HY-MES-EQU-012
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParam")]
        public async Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamDto request)
        {
            await _equipmentService.EquipmentProductProcessParamAsync(request);
        }

        /// <summary>
        /// 上料-原材料上料
        /// HY-MES-EQU-013
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Feeding")]
        public async Task FeedingLoadingAsync(FeedingLoadingDto request)
        {
            await _feedingService.FeedingLoadingAsync(request);
        }

        /// <summary>
        /// 卸料
        /// HY-MES-EQU-014
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Unloading")]
        public async Task FeedingUnloadingAsync(FeedingUnloadingDto request)
        {
            await _feedingService.FeedingUnloadingAsync(request);
        }

        /// <summary>
        /// 进站 HY-MES-EQU-015
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBound")]
        public async Task InBoundAsync(InBoundDto request)
        {
            await _inBoundService.InBoundAsync(request);
        }

        /// <summary>
        /// 进站（多个）HY-MES-EQU-016
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InBoundMore")]
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _inBoundService.InBoundMoreAsync(request);
        }

        /// <summary>
        /// 出站 HY-MES-EQU-017
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBound")]
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _outBoundService.OutBoundAsync(request);
        }

        /// <summary>
        /// 出站（多个） HY-MES-EQU-018
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutBoundMore")]
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _outBoundService.OutBoundMoreAsync(request);
        }

        ///// <summary>
        ///// 容器绑定
        ///// HY-MES-EQU-019
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("BindContainer")]
        //public async Task BindContainerAsync(BindContainerDto request)
        //{
        //    await _bindContainerService.BindContainerAsync(request);
        //}

        ///// <summary>
        ///// 条码绑定
        ///// HY-MES-EQU-020
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("BindSFCAsync")]
        //public async Task BindSFCAsync(BindSFCDto request)
        //{
        //    await _bindSFCService.BindSFCAsync(request);
        //}

        ///// <summary>
        ///// 条码解绑
        ///// HY-MES-EQU-021
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("UnBindSFCAsync")]
        //public async Task UnBindSFCAsync(UnBindSFCDto request)
        //{
        //    await _bindSFCService.UnBindSFCAsync(request);
        //}

        ///// <summary>
        ///// 容器解绑
        ///// HY-MES-EQU-022
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("UnBindContainer")]
        //public async Task UnBindContainerAsync(UnBindContainerDto request)
        //{
        //    await _bindContainerService.UnBindContainerAsync(request);
        //}


        ///// <summary>
        ///// 进站-容器
        ///// HY-MES-EQU-023 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("InboundInContainer")]
        //public async Task InboundInContainerAsync(InboundInContainerDto request)
        //{
        //    _logger.LogInformation("进站-容器：InboundInContainer,msg:{request}", request);
        //    await _inboundInContainerService.InboundInContainerAsync(request);
        //}

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        ///HY-MES-EQU-024 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateModuleSFC")]
        public async Task<GenerateModuleSFCModelDto> GenerateModuleSFCAsync(GenerateModuleSFCDto request)
        {
            return await _generateModuleSFCService.GenerateModuleSFCAsync(request);
        }

        ///// <summary>
        /////进站-电芯和托盘-装盘2
        ///// HY-MES-EQU-025 
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("InboundInSFCContainer")]
        //public async Task InboundInSFCContainerAsync(InboundInSFCContainerDto request)
        //{
        //    await _inboundInSFCContainerService.InboundInSFCContainerAsync(request);
        //}

        /// <summary>
        /// CCD文件上传完成
        /// HY-MES-EQU-026  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CCDFileUploadComplete")]
        public async Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteDto request)
        {
            _logger.LogInformation("CCD文件上传完成：CCDFileUploadComplete,msg:{request}", request);
            await _cCDFileUploadCompleteService.CCDFileUploadCompleteAsync(request);
        }

        ///// <summary>
        ///// 上报物料消耗
        /////HY-MES-EQU-027
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("FeedingConsumption")]
        //public async Task FeedingConsumptionAsync(FeedingConsumptionDto request)
        //{
        //    await _feedingConsumptionService.FeedingConsumptionAsync(request);
        //}

        /// <summary>
        /// 单体条码上料校验
        /// HY-MES-EQU-028
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SingleBarCodeLoadingVerification")]
        public async Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationDto request)
        {
            _logger.LogInformation("单体条码上料校验：SingleBarCodeLoadingVerification,msg:{request}", request);
            await _singleBarCodeLoadingVerificationService.SingleBarCodeLoadingVerificationAsync(request);
        }

        /// <summary>
        ///产出上报数量
        ///HY-MES-EQU-029   
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutPutQty")]
        public async Task OutPutQtyAsync(OutPutQtyDto request)
        {
            _logger.LogInformation("产出上报数量：OutPutQty,msg:{request}", request);
            await _outPutQtyService.OutPutQtyAsync(request);
        }

        ///// <summary>
        /////容器内条码查询
        /////HY-MES-EQU-035  
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("QueryContainerBindSfc")]
        //public async Task QueryContainerBindSfcAsync(QueryContainerBindSfcDto request)
        //{
        //    _logger.LogInformation("容器内条码查询：QueryContainerBindSfc,msg:{request}", request);
        //    await _queryContainerBindSfcService.QueryContainerBindSfcAsync(request);
        //}

        /// <summary>
        /// 获取NG数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNGData")]
        [ProducesResponseType(typeof(NGDataDto), 200)]
        public async Task<NGDataDto> GetNGDataAsync([FromQuery] NGDataQueryDto param)
        {
            return await _ngDataService.GetNGDataAsync(param);
        }

        /// <summary>
        /// 条码流转绑定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SfcCirculationBind")]
        public async Task SfcCirculationBindAsync(SfcCirculationBindDto request)
        {
            await _sfcCirculationService.SfcCirculationBindAsync(request);
        }

        /// <summary>
        /// 条码流转解绑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SfcCirculationUnBind")]
        public async Task SfcCirculationUnBindAsync(SfcCirculationUnBindDto request)
        {
            await _sfcCirculationService.SfcCirculationUnBindAsync(request, SfcCirculationTypeEnum.Merge);
        }

        /// <summary>
        /// 条码组件添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SfcCirculationModuleAdd")]
        public async Task SfcCirculationModuleAddAsync(SfcCirculationBindDto request)
        {
            await _sfcCirculationService.SfcCirculationModuleAddAsync(request);
        }

        /// <summary>
        /// 条码组件移除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SfcCirculationModuleRemove")]
        public async Task SfcCirculationModuleRemoveAsync(SfcCirculationUnBindDto request)
        {
            await _sfcCirculationService.SfcCirculationModuleRemoveAsync(request);
        }
    }
}
