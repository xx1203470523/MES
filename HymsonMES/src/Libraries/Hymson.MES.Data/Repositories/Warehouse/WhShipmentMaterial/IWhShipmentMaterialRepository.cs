using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Domain.WhShipmentMaterial;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Warehouse.Query;
using Hymson.MES.Data.Repositories.Warehouse.WhShipmentMaterial.View;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 仓储接口（出货单物料详情（外部数据））
    /// </summary>
    public interface IWhShipmentMaterialRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhShipmentMaterialEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhShipmentMaterialEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhShipmentMaterialEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WhShipmentMaterialEntity> entities);

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
        Task<WhShipmentMaterialEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhShipmentMaterialEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhShipmentMaterialEntity>> GetEntitiesAsync(WhShipmentMaterialQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhShipmentMaterialEntity>> GetPagedListAsync(WhShipmentMaterialPagedQuery pagedQuery);

    }
}
