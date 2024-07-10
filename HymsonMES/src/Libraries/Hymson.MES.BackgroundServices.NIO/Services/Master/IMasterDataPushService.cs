namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务接口（主数据）
    /// </summary>
    public interface IMasterDataPushService
    {
        /// <summary>
        /// 主数据（产品）
        /// </summary>
        /// <returns></returns>
        Task ProductAsync();

        /// <summary>
        /// 主数据（工站）
        /// </summary>
        /// <returns></returns>
        Task StationAsync();

        /// <summary>
        /// 主数据（控制项）
        /// </summary>
        /// <returns></returns>
        Task FieldAsync();

        /// <summary>
        /// 主数据（一次合格率目标）
        /// </summary>
        /// <returns></returns>
        Task PassrateTargetAsync();

        /// <summary>
        /// 主数据（环境监测）
        /// </summary>
        /// <returns></returns>
        Task EnvFieldAsync();

        /// <summary>
        /// 主数据（人员资质）
        /// </summary>
        /// <returns></returns>
        Task PersonCertAsync();

        /// <summary>
        /// 主数据（排班）
        /// </summary>
        /// <returns></returns>
        Task TeamSchedulingAsync();

    }
}
