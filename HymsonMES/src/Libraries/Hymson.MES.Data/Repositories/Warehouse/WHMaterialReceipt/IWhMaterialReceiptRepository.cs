using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Query;

namespace Hymson.MES.Data.Repositories.WHMaterialReceipt
{
    /// <summary>
    /// 仓储接口（物料收货表）
    /// </summary>
    public interface IWhMaterialReceiptRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhMaterialReceiptEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhMaterialReceiptEntity> entities);


        /// <summary>
        /// 创建详细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertDetailAsync(List<WHMaterialReceiptDetailEntity> entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhMaterialReceiptEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WhMaterialReceiptEntity> entities);

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


        Task<int> DeletesDetailByIdAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialReceiptEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialReceiptEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialReceiptEntity>> GetEntitiesAsync(WhMaterialReceiptQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialReceiptEntity>> GetPagedListAsync(WhMaterialReceiptPagedQuery pagedQuery);


    }
}
