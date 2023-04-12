/*
 *creator: Karl
 *
 *describe: 条码流转表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:50:00
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码流转表仓储接口
    /// </summary>
    public interface IManuSfcCirculationRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcCirculationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetBySfcAsync(ManuSfcCirculationQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcCirculationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationEntitiesAsync(ManuSfcCirculationQuery manuSfcCirculationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcCirculationEntity>> GetPagedInfoAsync(ManuSfcCirculationPagedQuery manuSfcCirculationPagedQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcCirculationEntity manuSfcCirculationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcCirculationEntity manuSfcCirculationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);
    }
}
