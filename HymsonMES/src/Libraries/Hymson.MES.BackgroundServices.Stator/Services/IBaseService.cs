namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public interface IBaseService
    {
        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <returns></returns>
        Task<BaseStatorBo> GetStatorBaseConfigAsync();

        /// <summary>
        /// 保存转换数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataAsync<T>(IEnumerable<T> entities) where T : BaseOPEntity;

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataAsync(StatorSummaryBo summaryBo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="buzKey"></param>
        /// <param name="waterLevel"></param>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataWithCommitAsync(string buzKey, long waterLevel, StatorSummaryBo summaryBo);

    }
}
