using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquSpareParts.View;
using Hymson.MES.Data.Repositories.Equipment.Query;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储接口（备件注册表）
    /// </summary>
    public interface IEquSparePartsRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSparePartsEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<EquSparePartsEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSparePartsEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<EquSparePartsEntity> entities);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartsEntity>> GetByReIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparePartsEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartsEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartsEntity>> GetEntitiesAsync(EquSparePartsQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartsEntity>> GetPagedInfoAsync(EquSparePartsPagedQuery pagedQuery);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquSparePartsEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 更新（备件关联备件类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateTypeAsync(UpdateSparePartsTypeEntity entity);

        /// <summary>
        /// 分页查询(过滤掉已有类型的备件)
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparePartsEntity>> GetPagedInfoNotWithTypeoAsync(EquSparePartsPagedQuery pagedQuery);

        /// <summary>
        /// 获取关联的备件
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSparePartsEntity>> GetSparePartsGroupRelationAsync(long Id);

        /// <summary>
        /// 更新（清空备件关联备件类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> CleanTypeAsync(UpdateSparePartsTypeEntity entity);


        /// <summary>
        /// 更新（数量）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAddQtyAsync(UpdateSparePartsQtyEntity entity);

        /// <summary>
        /// 更新（数量）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns> 
        Task<int> UpdateMinusQtyAsync(UpdateSparePartsQtyEntity entity);

    }
}
