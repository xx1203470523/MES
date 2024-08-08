using Hymson.MES.Core.Domain.Process;

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
        /// <param name="barCodes"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataAsync<T>(IEnumerable<T> entities, IEnumerable<string> barCodes) where T : BaseOPEntity;

        /// <summary>
        /// 获取参数编码
        /// </summary>
        /// <param name="parameterCodes"></param>
        /// <param name="statorBo"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetParameterEntitiesWithInitAsync(IEnumerable<string> parameterCodes, BaseStatorBo statorBo);

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
