namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务接口（业务数据）
    /// </summary>
    public interface IBuzDataPushService
    {
        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <returns></returns>
        Task CollectionAsync();

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <returns></returns>
        Task ProductionAsync();

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <returns></returns>
        Task MaterialAsync();

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <returns></returns>
        Task PassrateProductAsync();

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <returns></returns>
        Task PassrateStationAsync();

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <returns></returns>
        Task DataEnvAsync();

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <returns></returns>
        Task IssueAsync();

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <returns></returns>
        Task WorkOrderAsync();

        /// <summary>
        /// 业务数据（通用业务）
        /// </summary>
        /// <returns></returns>
        Task CommonAsync();

        /// <summary>
        /// 业务数据（附件）
        /// </summary>
        /// <returns></returns>
        Task AttachmentAsync();

    }
}
