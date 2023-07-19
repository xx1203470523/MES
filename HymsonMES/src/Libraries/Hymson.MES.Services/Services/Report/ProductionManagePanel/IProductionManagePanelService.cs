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
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<ProductionManagePanelModuleDto> GetModuleAchievingInfoAsync(long siteId);
    }
}
