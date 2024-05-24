using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（条码关系表）
    /// </summary>
    public interface IManuBarCodeRelationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuBarCodeRelationEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuBarCodeRelationEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuBarCodeRelationEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuBarCodeRelationEntity> entities);

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
        Task<ManuBarCodeRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBarCodeRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBarCodeRelationEntity>> GetEntitiesAsync(ManuBarcodeRelationQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBarCodeRelationEntity>> GetPagedListAsync(ManuBarcodeRelationPagedQuery pagedQuery);


        /// <summary>
        /// 根据条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBarCodeRelationEntity>> GetSfcMoudulesAsync(ManuComponentBarcodeRelationQuery query);

        /// <summary>
        /// 查询条码
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBarCodeRelationEntity>> GetManuBarCodeRelationEntitiesAsync(ManuSfcProduceQuery query);

        /// <summary>
        /// 条码关系拆解
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ManuBarCodeRelationUpdateAsync(DManuBarCodeRelationCommand command);

        /// <summary>
        /// 根据Location查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBarCodeRelationEntity>> GetByLocationAsync(ManuComponentBarcodeRelationLocationQuery query);

        /// <summary>
        /// 条码关系表拆解移除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DisassemblyUpdateAsync(DisassemBarCodeRelationblyCommand command);

        /// <summary>
        /// 组件使用报告 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBarCodeRelationEntity>> GetReportPagedInfoAsync(ComUsageReportPagedQuery queryParam);
    }
}
