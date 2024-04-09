﻿using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 条码操作控制器(进站)
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcOperateController : ControllerBase
    {
        /// <summary>
        /// 生产服务接口
        /// </summary>
        private readonly IManuSfcOperateService _manuSfcOperateService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuSfcOperateService"></param>
        public ManuSfcOperateController(IManuSfcOperateService manuSfcOperateService)
        {
            _manuSfcOperateService = manuSfcOperateService;
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStation")]
        [LogDescription("进站", BusinessType.OTHER, "InStation", ReceiverTypeEnum.MES)]
        public async Task InBoundAsync(InBoundDto request)
        {
            await _manuSfcOperateService.InBoundAsync(request);
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InStationMore")]
        [LogDescription("多个进站", BusinessType.OTHER, "InStationMore", ReceiverTypeEnum.MES)]
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _manuSfcOperateService.InBoundMoreAsync(request);
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStation")]
        [LogDescription("出站", BusinessType.OTHER, "OutStation", ReceiverTypeEnum.MES)]
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _manuSfcOperateService.OutBoundAsync(request);
        }

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OutStationMore")]
        [LogDescription("多个出站", BusinessType.OTHER, "OutStationMore", ReceiverTypeEnum.MES)]
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _manuSfcOperateService.OutBoundMoreAsync(request);
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
            await _manuSfcOperateService.InBoundCarrierAsync(request);
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
            await _manuSfcOperateService.OutBoundCarrierAsync(request);
        }

        /// <summary>
        /// 分页查询列表（PDA条码出站）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<ManuSfcInstationPagedQueryOutputDto>> GetPagedListAsync([FromQuery] ManuSfcInstationPagedQueryDto pagedQueryDto)
        {
            return await _manuSfcOperateService.GetPagedListAsync(pagedQueryDto);
        }
    }
}
