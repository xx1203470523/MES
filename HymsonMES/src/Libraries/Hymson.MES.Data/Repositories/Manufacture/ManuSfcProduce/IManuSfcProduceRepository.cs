using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.View;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除）仓储接口
    /// </summary>
    public interface IManuSfcProduceRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceView>> GetPagedInfoAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery);

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> QualityLockAsync(QualityLockCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcProduceEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcQuery"></param>
        /// <returns></returns>
        Task<ManuSfcProduceEntity> GetBySFCAsync(ManuSfcProduceBySfcQuery sfcQuery);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcProduceQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetManuSfcProduceEntitiesAsync(ManuSfcProduceQuery manuSfcProduceQuery);

        /// <summary>
        /// 获取带manu_sfc_info的list
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceInfoView>> GetManuSfcProduceInfoEntitiesAsync(ManuSfcProduceQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcProduceEntity manuSfcProduceEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcProduceEntity manuSfcProduceEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(IEnumerable<long> ids);

        /// <summary>
        /// 删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<int> DeletePhysicalAsync(DeletePhysicalBySfcCommand sfcCommand);

        /// <summary>
        /// 批量删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<int> DeletePhysicalRangeAsync(DeletePhysicalBySfcsCommand sfcsCommand);

        /// <summary>
        /// 批量更新条码IsScrap
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateIsScrapAsync(UpdateIsScrapCommand command);

        /// <summary>
        /// 批量更新条码工艺路线和工序信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateRouteAsync(ManuSfcUpdateRouteCommand command);

        /// <summary>
        /// 更新条码Status
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(UpdateStatusCommand command);

        /// <summary>
        /// 更新工序ProcedureId
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateProcedureIdAsync(UpdateProcedureCommand command);

        /// <summary>
        /// 根据SFC批量更新工序与状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateProcedureAndStatusRangeAsync(UpdateProcedureAndStatusCommand command);

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> LockedSfcProcedureAsync(LockedProcedureCommand command);

        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UnLockedSfcProcedureAsync(UnLockedProcedureCommand command);

        /// <summary>
        /// 插入或者更新
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        Task<int> InsertOrUpdateSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys);

        /// <summary>
        /// 新增在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        Task<int> InsertSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity);

        /// <summary>
        /// 批量新增在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        Task<int> InsertSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys);

        /// <summary>
        /// 更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        Task<int> UpdatetSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity);

        /// <summary>
        /// 批量更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatestSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys);

        /// <summary>
        /// 根据ID获取在制品业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCIdAsync(long id);

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCAsync(SfcProduceBusinessQuery query);

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceBusinessView>> GetSfcProduceBusinessListBySFCAsync(SfcListProduceBusinessQuery sfc);

        /// <summary>
        /// 根据IDs批量获取在制品业务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetSfcProduceBusinessBySFCIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteSfcProduceBusinessBySfcInfoIdAsync(DeleteSfcProduceBusinesssBySfcInfoIdCommand command);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="sfcInfoIds"></param>
        /// <returns></returns>
        Task<int> DeleteSfcProduceBusinesssAsync(DeleteSfcProduceBusinesssCommand command);
    }
}
