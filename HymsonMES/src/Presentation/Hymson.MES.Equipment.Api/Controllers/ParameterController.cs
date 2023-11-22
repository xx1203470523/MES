﻿using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 控制器（参数）
    /// @author wangkeming
    /// @date 2023-08-07
    /// </summary>
    [ApiController]
    //[AllowAnonymous]
    [Route("EquipmentService/api/v1/Collection")]
    public class ParameterController : ControllerBase
    {
        /// <summary>
        /// 过程参数采集
        /// </summary>
        private readonly IProcessCollectionService _processCollectionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processCollectionService"></param>
        public ParameterController(IProcessCollectionService processCollectionService)
        {
            _processCollectionService = processCollectionService;
        }


        /// <summary>
        ///产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("product")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task ProductCollectionAsync(ProductProcessParameterDto request)
        {
            await _processCollectionService.ProductCollectionAsync(request);
        }

        /// <summary>
        ///设备过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("equipment")]
        [ProducesDefaultResponseType(typeof(ResultDto))]
        public async Task EquipmentCollectionAsync(IEnumerable<EquipmentProcessParameterDto> request)
        {
            await _processCollectionService.EquipmentCollectionAsync(request);
        }

    }
}