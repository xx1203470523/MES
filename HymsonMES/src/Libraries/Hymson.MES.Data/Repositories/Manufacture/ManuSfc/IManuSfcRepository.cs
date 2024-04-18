using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;

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

        /// <summary>
        /// 部分报废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> PartialScrapmanuSFCByIdAsync(IEnumerable<ManuSFCPartialScrapByIdCommand> commands);

        Task<ManuSfcEntity> GetSingleAsync(ManuSfcQuery query);
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
