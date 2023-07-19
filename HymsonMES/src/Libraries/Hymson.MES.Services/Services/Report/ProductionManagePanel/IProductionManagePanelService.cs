using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.ProductionManagePanel
{
    /// <summary>
    /// 生产管理看板服务
    /// </summary>
    public interface IProductionManagePanelService
    {
        Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId);
    }
}
