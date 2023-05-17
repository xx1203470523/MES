using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Request.Equipment;
using Hymson.MES.EquipmentServices.Request.Feeding;
using Hymson.MES.EquipmentServices.Request.FeedingConsumption;
using Hymson.MES.EquipmentServices.Request.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Request.InboundInContainer;
using Hymson.MES.EquipmentServices.Request.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Request.OutPutQty;
using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete;
using Hymson.MES.EquipmentServices.Services.Equipment;
using Hymson.MES.EquipmentServices.Services.FeedingConsumption;
using Hymson.MES.EquipmentServices.Services.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Services.InboundInContainer;
using Hymson.MES.EquipmentServices.Services.InboundInSFCContainer;
using Hymson.MES.EquipmentServices.Services.OutPutQty;
using Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 设备接口（海目星设备）
    /// </summary>
    [Route("api/v1/EquApi")]
    [ApiController]
    public class EquipmentController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquipmentController> _logger;

        /// <summary>
        /// 业务接口（设备监控服务）
        /// </summary>
        private readonly IEquipmentMonitorService _equipmentService;
        private readonly IBindSFCService _bindSFCService;
        private readonly IBindContainerService _bindContainerService;
        private readonly ICCDFileUploadCompleteService _cCDFileUploadCompleteService;
        private readonly IFeedingConsumptionService _feedingConsumptionService;
        private readonly IGenerateModuleSFCService _generateModuleSFCService;
        private readonly IInboundInContainerService _inboundInContainerService;
        private readonly IInboundInSFCContainerService _inboundInSFCContainerService;
        private readonly IOutPutQtyService _outPutQtyService;
        private readonly IQueryContainerBindSfcService _queryContainerBindSfcService;
        private readonly ISingleBarCodeLoadingVerificationService _singleBarCodeLoadingVerificationService;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equipmentService"></param>
        /// <param name="bindSFCService"></param>
        /// <param name="bindContainerService"></param>
        /// <param name="cCDFileUploadCompleteService"></param>
        /// <param name="feedingConsumptionService"></param>
        /// <param name="generateModuleSFCService"></param>
        /// <param name="inboundInContainerService"></param>
        /// <param name="inboundInSFCContainerService"></param>
        /// <param name="outPutQtyService"></param>
        /// <param name="queryContainerBindSfcService"></param>
        /// <param name="singleBarCodeLoadingVerificationService"></param>
        public EquipmentController(ILogger<EquipmentController> logger,
            IEquipmentMonitorService equipmentService,
            IBindSFCService bindSFCService,
            IBindContainerService bindContainerService,
            ICCDFileUploadCompleteService cCDFileUploadCompleteService,
            IFeedingConsumptionService feedingConsumptionService,
            IGenerateModuleSFCService generateModuleSFCService,
            IInboundInContainerService inboundInContainerService,
            IInboundInSFCContainerService inboundInSFCContainerService,
            IOutPutQtyService outPutQtyService,
            IQueryContainerBindSfcService queryContainerBindSfcService,
            ISingleBarCodeLoadingVerificationService singleBarCodeLoadingVerificationService)
        {
            _logger = logger;
            _equipmentService = equipmentService;
            _bindSFCService = bindSFCService;
            _bindContainerService = bindContainerService;
            _cCDFileUploadCompleteService = cCDFileUploadCompleteService;
            _feedingConsumptionService = feedingConsumptionService;
            _generateModuleSFCService = generateModuleSFCService;
            _inboundInContainerService = inboundInContainerService;
            _inboundInSFCContainerService = inboundInSFCContainerService;
            _outPutQtyService = outPutQtyService;
            _queryContainerBindSfcService = queryContainerBindSfcService;
            _singleBarCodeLoadingVerificationService = singleBarCodeLoadingVerificationService;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Heartbeat")]
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request)
        {
            await _equipmentService.EquipmentHeartbeatAsync(request);
        }

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("State")]
        public async Task EquipmentStateAsync(EquipmentStateRequest request)
        {
            await _equipmentService.EquipmentStateAsync(request);
        }

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Alarm")]
        public async Task EquipmentAlarmAsync(EquipmentAlarmRequest request)
        {
            await _equipmentService.EquipmentAlarmAsync(request);
        }

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("DownReason")]
        public async Task EquipmentDownReasonAsync(EquipmentDownReasonRequest request)
        {
            await _equipmentService.EquipmentDownReasonAsync(request);
        }


        /// <summary>
        /// 设备过程参数采集（无在制品条码）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProcessParam")]
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamRequest request)
        {
            await _equipmentService.EquipmentProcessParamAsync(request);
        }

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParamInNotCanSFC")]
        public async Task EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCRequest request)
        {
            await _equipmentService.EquipmentProductProcessParamInNotCanSFCAsync(request);
        }

        /// <summary>
        /// 设备产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParam")]
        public async Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamRequest request)
        {
            await _equipmentService.EquipmentProductProcessParamAsync(request);
        }


        /// <summary>
        /// 上料-原材料上料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Feeding")]
        public async Task FeedingLoadingAsync(FeedingLoadRequest request)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 卸料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Unloading")]
        public async Task FeedingUnloadingAsync(FeedingUnloadingRequest request)
        {
            await Task.CompletedTask;
        }


        /// <summary>
        /// 条码绑定
        /// HY-MES-EQU-019
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        public async Task BindContainerAsync(BindContainerRequest request)
        {
            await _bindContainerService.BindContainerAsync(request);
        }

        /// <summary>
        /// 条码绑定
        /// HY-MES-EQU-020
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindSFCAsync")]
        public async Task BindSFCAsync(BindSFCRequest request)
        {
            await _bindSFCService.BindSFCAsync(request);
        }

        /// <summary>
        /// 条码解绑
        /// HY-MES-EQU-020
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindSFCAsync")]
        public async Task UnBindSFCAsync(UnBindSFCRequest request)
        {
            await _bindSFCService.UnBindSFCAsync(request);
        }

        /// <summary>
        /// 容器解绑
        /// HY-MES-EQU-022
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindContainer")]
        public async Task UnBindContainerAsync(UnBindContainerRequest request)
        {
            await _bindContainerService.UnBindContainerAsync(request);
        }


        /// <summary>
        /// 进站-容器
        /// HY-MES-EQU-023 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInContainer")]
        public async Task InboundInContainerAsync(InboundInContainerRequest request)
        {
            await _inboundInContainerService.InboundInContainerAsync(request);
        }

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        ///HY-MES-EQU-024 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateModuleSFC")]
        public async Task GenerateModuleSFCAsync(GenerateModuleSFCRequest request)
        {
            await _generateModuleSFCService.GenerateModuleSFCAsync(request);
        }

        /// <summary>
        ///进站-电芯和托盘-装盘2
        /// HY-MES-EQU-025 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInSFCContainer")]
        public async Task InboundInSFCContainerAsync(InboundInSFCContainerRequest request)
        {
            await _inboundInSFCContainerService.InboundInSFCContainerAsync(request);
        }


        /// <summary>
        /// CCD文件上传完成
        /// HY-MES-EQU-026  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CCDFileUploadComplete")]
        public async Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteRequest request)
        {
            await _cCDFileUploadCompleteService.CCDFileUploadCompleteAsync(request);
        }

        /// <summary>
        /// 上报物料消耗
        ///HY-MES-EQU-027
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FeedingConsumption")]
        public async Task FeedingConsumptionAsync(FeedingConsumptionRequest request)
        {
            await _feedingConsumptionService.FeedingConsumptionAsync(request);
        }


        /// <summary>
        /// 单体条码上料校验
        /// HY-MES-EQU-028
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SingleBarCodeLoadingVerification")]
        public async Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationRequest request)
        {
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
        public async Task OutPutQtyAsync(OutPutQtyRequest request)
        {
            await _outPutQtyService.OutPutQtyAsync(request);
        }


        /// <summary>
        ///容器内条码查询
        ///HY-MES-EQU-035  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryContainerBindSfc")]
        public async Task QueryContainerBindSfcAsync(QueryContainerBindSfcRequest request)
        {
            await _queryContainerBindSfcService.QueryContainerBindSfcAsync(request);
        }

    }
}