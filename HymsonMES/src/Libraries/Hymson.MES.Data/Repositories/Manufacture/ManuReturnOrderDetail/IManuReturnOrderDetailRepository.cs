using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrderDetail.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（生产退料单明细）
    /// </summary>
    public interface IManuReturnOrderDetailRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuReturnOrderDetailEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuReturnOrderDetailEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuReturnOrderDetailEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuReturnOrderDetailEntity> entities);

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
        Task<ManuReturnOrderDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuReturnOrderDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuReturnOrderDetailEntity>> GetEntitiesAsync(ManuReturnOrderDetailQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuReturnOrderDetailEntity>> GetPagedListAsync(ManuReturnOrderDetailPagedQuery pagedQuery);

        /// <summary>
        /// 根据Id批量更新状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateManuReturnOrderDetailIsReceivedByIdRangeAsync(IEnumerable<UpdateManuReturnOrderDetailIsReceivedByIdCommand> commands);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ReportReturnOrderResultDto>> GetReportPagedInfoAsync(ReportReturnOrderQueryDto param);
    }
}
