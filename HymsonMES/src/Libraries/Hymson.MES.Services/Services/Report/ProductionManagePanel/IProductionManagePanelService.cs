using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.ProductionManagePanel
{
    /// <summary>
    /// 生产管理看板服务
    /// </summary>
    public interface IProductionManagePanelService
    {
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId, string procedureCode);
        /// <summary>
        /// 获取模组达成信息明细
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductionManagePanelModuleDto>> GetModuleAchievingInfoAsync(ModuleAchievingQueryDto param);
        /// <summary>
        /// 获取模组达成信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<dynamic>> GetModuleInfoDynamicAsync(ModuleAchievingQueryDto param);

        /// <summary>
        /// 获取当天工序直通率和良品率
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessQualityRateDto>> GetProcessQualityRateAsync(ProcessQualityRateQueryDto param);

        /// <summary>
        /// 获取工序良品趋势
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessYieldRateDto>> GetProcessYieldRateAsync(ProcessYieldRateQueryDto param);

        /// <summary>
        /// 获取工序指数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessIndicatorsDto>> GetProcessIndicatorsAsync(ProcessIndicatorsQueryDto param);

        /// <summary>
        /// 获取设备稼动率信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentUtilizationRateDto>> GetEquipmentUtilizationRateAsync(EquipmentUtilizationRateQueryDto param);

        /// <summary>
        /// 获取每日工序Top3不良（不良率和不良数）
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ManuSummaryBadRecordTop3Dto>> getUnqualifiedTop3(ManuSummaryBadRecordTop3QueryDto query);

        /// <summary>
        /// 获取每月Top3不良上报
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ManuSummarySfcBadRecordTop3Dto>> GetBadTop3Async();
    }
}
