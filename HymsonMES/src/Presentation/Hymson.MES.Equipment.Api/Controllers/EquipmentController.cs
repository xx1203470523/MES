using Hymson.MES.EquipmentServices.Request.BindContainer;
using Hymson.MES.EquipmentServices.Request.BindSFC;
using Hymson.MES.EquipmentServices.Request.Equipment;
using Hymson.MES.EquipmentServices.Request.Feeding;
using Hymson.MES.EquipmentServices.Services.BindContainer;
using Hymson.MES.EquipmentServices.Services.BindSFC;
using Hymson.MES.EquipmentServices.Services.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 设备
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
        /// 业务接口（设备）
        /// </summary>
        private readonly IEquipmentService _equipmentService;
        private readonly IBindSFCService _bindSFCService;
        private readonly IBindContainerService _bindContainerService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equipmentService"></param>
        /// <param name="bindSFCService"></param>
        /// <param name="bindContainerService"></param>
        public EquipmentController(ILogger<EquipmentController> logger,
            IEquipmentService equipmentService,
            IBindSFCService bindSFCService,
            IBindContainerService bindContainerService)
        {
            _logger = logger;
            _equipmentService = equipmentService;
            _bindSFCService = bindSFCService;
            _bindContainerService = bindContainerService;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
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
            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParamInNotCanSFC")]
        public async Task EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCRequest request)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EquipmentProductProcessParam")]
        public async Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamRequest request)
        {
            await Task.CompletedTask;
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

    }
}