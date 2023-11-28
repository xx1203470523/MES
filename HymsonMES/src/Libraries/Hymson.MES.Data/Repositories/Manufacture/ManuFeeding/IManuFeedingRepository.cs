using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 仓储接口（物料加载）
    /// </summary>
    public interface IManuFeedingRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFeedingEntity entity);

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateQtyByIdAsync(UpdateFeedingQtyByIdCommand command);

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateFeedingQtyByIdAsync(IEnumerable<UpdateFeedingQtyByIdCommand> commands);

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteByIdsAsync(long[] ids);

        /// <summary>
        /// 根据Code和物料ID查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuFeedingEntity> GetByBarCodeAndMaterialIdAsync(GetByBarCodeAndMaterialIdQuery query);

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdAsync(GetByResourceIdAndMaterialIdQuery query);

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdsAsync(GetByResourceIdAndMaterialIdsQuery query);

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByFeedingPointIdAndMaterialIdsAsync(GetByFeedingPointIdAndMaterialIdsQuery query);

        /// <summary>
        /// 获取加载数据列表（只读取剩余数量大于0的）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdsWithOutZeroAsync(GetByResourceIdAndMaterialIdsQuery query);

        /// <summary>
        /// 获取加载数据列表（只读取剩余数量大于0的）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByFeedingPointIdWithOutZeroAsync(GetByFeedingPointIdsQuery query);

        /// <summary>
        /// 根据上料点Id与资源IDs获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFeedingEntity>> GetByLoadIdAndResourceIdsSqlAsync(GetByLoadIdAndResourceIdsQuery query);

    }
}
