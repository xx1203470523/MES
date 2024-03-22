using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表仓储接口
    /// </summary>
    public interface IManuSfcRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcEntity manuSfcEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcEntity> manuSfcEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcEntity manuSfcEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcEntity> manuSfcEntitys);

        /// <summary>
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeWithStatusCheckAsync(IEnumerable<ManuSfcEntity>? entities);

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
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(EntityBySFCsQuery manuSfcQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetAllManuSfcEntitiesAsync(EntityBySFCsQuery manuSfcQuery);

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcView>> GetManuSfcInfoEntitiesAsync(ManuSfcStatusQuery param);

        /// <summary>
        /// 批量更新条码状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ManuSfcUpdateCommand command);

        /// <summary>
        /// 批量更新条码（使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> MultiUpdateSfcIsUsedAsync(MultiSfcUpdateIsUsedCommand command);

        /// <summary>
        /// 批量更新条码（条码状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> MultiUpdateSfcStatusAsync(MultiSFCUpdateStatusCommand command);

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcEntity> GetBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 获取SFC（批量）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetAllBySFCsAsync(EntityBySFCsQuery query);

        /// <summary>
        /// 获取在制SFC（批量）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetProduceBySFCsAsync(EntityBySFCsQuery query);

        /// <summary>
        /// 更具sfc 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(IEnumerable<string> sfcs);


        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceView>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery);

        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceSelectView>> GetManuSfcSelectPagedInfoAsync(ManuSfcProduceSelectPagedQuery query);

        /// <summary>
        /// 分页查询（查询所有条码信息-不查询在制表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcAboutInfoView>> GetManuSfcAboutInfoPagedAsync(ManuSfcAboutInfoPagedQuery query);

        /// <summary>
        /// 根据SFC查询条码信息-不查询在制表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcAboutInfoView> GetManSfcAboutInfoBySfcAsync(ManuSfcAboutInfoBySfcQuery query);

        /// <summary>
        /// 批量更新条码（条码状态与使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateSfcStatusAndIsUsedAsync(ManuSfcUpdateStatusAndIsUsedCommand command);

        /// <summary>
        /// 批量更新进站条码状态和在用状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> InStationManuSfcByIdAsync(IEnumerable<InStationManuSfcByIdCommand> commands);

        /// <summary>
        /// 批量更新条码（每个条码状态不一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ManuSfcUpdateStatusBySfcsAsync(IEnumerable<ManuSfcUpdateStatusCommand> commands);

        /// <summary>
        /// 批量更新条码（更具Id 状态更新为一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ManuSfcUpdateStatuByIdsAsync(ManuSfcUpdateStatusByIdsCommand command);

        /// <summary>
        /// 批量更新条码（更具Id 状态更新为一致）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> ManuSfcUpdateStatuByIdRangeAsync(IEnumerable<ManuSfcUpdateStatusByIdCommand> commands);

        /// <summary>
        /// 批量更新条码（更具Id 状态更新为一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ManuSfcUpdateStatuByIdAsync(ManuSfcUpdateStatusByIdCommand command);

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> ManuSfcScrapByIdsAsync(IEnumerable<ScrapManuSfcByIdCommand> commands);

        /// <summary>
        /// 取消条码报废
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> ManuSfcCancellScrapByIdsAsync(IEnumerable<CancelScrapManuSfcByIdCommand> commands);

        /// <summary>
        /// 更新条码数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateManuSfcQtyByIdRangeAsync(IEnumerable<UpdateManuSfcQtyByIdCommand> commands);

        /// <summary>
        /// 根据SFCs设置条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAndQtyBySfcsAsync(UpdateStatusAndQtyBySfcsCommand command);

        /// <summary>
        /// 根据Id条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAndQtyByIdAsync(UpdateStatusAndQtyByIdCommand command);

        /// <summary>
        /// 根据Id条码数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateManuSfcQtyAndCurrentQtyVerifyByIdAsync(UpdateManuSfcQtyAndCurrentQtyVerifyByIdCommand command);
        Task<ManuSfcEntity> GetOneAsync(ManuSfcQuery query);
        Task<IEnumerable<ManuSfcEntity>> GetListAsync(ManuSfcQuery query);
        #endregion

        #region 顷刻

        /// <summary>
        /// 根据SFCs设置条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command);

        #endregion

    }
}
