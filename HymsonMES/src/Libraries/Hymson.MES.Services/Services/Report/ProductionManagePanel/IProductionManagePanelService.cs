using Hymson.MES.Core.Domain.Process;
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
        /// <returns></returns>
        Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId);
        /// <summary>
        /// 获取模组达成信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductionManagePanelModuleDto>> GetModuleAchievingInfoAsync(ModuleAchievingQueryDto param);

        /// <summary>
        /// 获取当天工序直通率和良品率
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessQualityRateDto>> GetProcessQualityRateAsync(ProcessQualityRateQuery param);

        /// <summary>
        /// 获取工序良品趋势
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcessYieldRateDto>> GetProcessYieldRateAsync(ProcessYieldRateQuery param);
    }
}
