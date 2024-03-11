using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhShipment;
using Hymson.MES.Core.Domain.WhShipmentBarcode;
using Hymson.MES.Core.Domain.WhShipmentMaterial;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhShipment.View;
using Hymson.MES.Data.Repositories.WhShipment.Query;

namespace Hymson.MES.Data.Repositories.WhShipment
{
    /// <summary>
    /// 仓储接口（出货单）
    /// </summary>
    public interface IWhShipmentRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhShipmentEntity entity);

        /// <summary>
        /// INSERT BARCORDS
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhShipmentBarcodeEntity> entity);
        /// <summary>
        /// INSERT DETAIL
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhShipmentMaterialEntity> entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhShipmentEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhShipmentEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WhShipmentEntity> entities);

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
        /// DELETE DETAIL
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesDetailByIdAsync(long[] ids);

        /// <summary>
        /// DELETE BARCORDS
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesBarcodeByDetailIdAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhShipmentEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhShipmentEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhShipmentView> GetEntityWithCodeByIdAsync(long id);

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<WhShipmentView> GetEntityAsync(WhShipmentQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhShipmentEntity>> GetEntitiesAsync(WhShipmentQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhShipmentEntity>> GetPagedListAsync(WhShipmentPagedQuery pagedQuery);

    }
}
