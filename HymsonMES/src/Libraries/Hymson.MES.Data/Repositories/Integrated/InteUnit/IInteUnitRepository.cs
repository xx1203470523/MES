/*
 *creator: Karl
 *
 *describe: 单位表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-29 02:13:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 单位表仓储接口
    /// </summary>
    public interface IInteUnitRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteUnitEntity inteUnitEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteUnitEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteUnitEntity> inteUnitEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteUnitEntity inteUnitEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteUnitEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteUnitEntity> inteUnitEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteUnitEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteUnitEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteUnitQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteUnitEntity>> GetInteUnitEntitiesAsync(InteUnitQuery inteUnitQuery);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<InteUnitEntity> GetByCodeAsync(EntityByCodeQuery param);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteUnitPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteUnitEntity>> GetPagedInfoAsync(InteUnitPagedQuery inteUnitPagedQuery);
        #endregion
    }
}
