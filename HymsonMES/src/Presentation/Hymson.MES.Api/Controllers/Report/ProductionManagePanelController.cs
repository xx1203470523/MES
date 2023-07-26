﻿using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.ProductionManagePanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 生产管理看板控制器
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductionManagePanelController : ControllerBase
    {
        private readonly IProductionManagePanelService _productionManagePanelService;
        public ProductionManagePanelController(IProductionManagePanelService productionManagePanelService)
        {
            _productionManagePanelService = productionManagePanelService;
        }
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getOverallInfo")]
        public async Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId)
        {
            return await _productionManagePanelService.GetOverallInfoAsync(siteId);
        }

        /// <summary>
        /// 获取当天模组达成数据
        /// </summary>
        /// <param name="param">站点ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getModuleAchievingInfo")]
        public async Task<IEnumerable<ProductionManagePanelModuleDto>> GetModuleAchievingInfoAsync([FromQuery] ModuleAchievingQueryDto param)
        {
            return await _productionManagePanelService.GetModuleAchievingInfoAsync(param);
        }
        /// <summary>
        /// 获取模组达成详细信息
        /// </summary>
        /// <param name="param">/param>
        /// <returns></returns>
        [HttpGet]
        [Route("getModuleInfoDynamic")]
        public async Task<List<dynamic>> GetModuleInfoDynamicAsync([FromQuery] ModuleAchievingQueryDto param)
        {
            return await _productionManagePanelService.GetModuleInfoDynamicAsync(param);
        }
        /// <summary>
        /// 获取工序稼动率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessUtilizationRate")]
        public async Task<IEnumerable<EquipmentUtilizationRateDto>> GetProcessUtilizationRateAsync([FromQuery] EquipmentUtilizationRateQuery param)
        {
            return await _productionManagePanelService.GetEquipmentUtilizationRateAsync(param);
        }

        /// <summary>
        /// 获取工序良品率相关信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessQualityRate")]
        public async Task<IEnumerable<ProcessQualityRateDto>> GetProcessQualityRateAsync([FromQuery] ProcessQualityRateQuery param)
        {
            return await _productionManagePanelService.GetProcessQualityRateAsync(param);
        }

        /// <summary>
        /// 获取工序良品率波动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessYieldRate")]
        public async Task<IEnumerable<ProcessYieldRateDto>> GetProcessYieldRateAsync([FromQuery] ProcessYieldRateQuery param)
        {
            return await _productionManagePanelService.GetProcessYieldRateAsync(param);
        }

        /// <summary>
        /// 获取工序指数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessIndicators")]
        public async Task<IEnumerable<ProcessIndicatorsDto>> GetProcessIndicatorsAsync([FromQuery] ProcessIndicatorsQuery param)
        {
            return await _productionManagePanelService.GetProcessIndicatorsAsync(param);
        }
    }
}
