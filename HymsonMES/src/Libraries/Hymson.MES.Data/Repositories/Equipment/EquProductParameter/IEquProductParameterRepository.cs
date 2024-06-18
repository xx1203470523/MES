using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备生产参数仓储接口
    /// </summary>
    public interface IEquProductParameterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquProductParameterEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquProductParameterEntity> entities);

    }
}
