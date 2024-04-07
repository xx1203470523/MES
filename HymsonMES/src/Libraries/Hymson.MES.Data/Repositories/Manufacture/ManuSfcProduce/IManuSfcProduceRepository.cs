using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除）仓储接口
    /// </summary>
    public partial interface IManuSfcProduceRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceView>> GetPagedInfoAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceEntity>> GetPagedListAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery);

        /// <summary>
        /// 分页查询（在制）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceEntity>> GetPagedListAsync(ManuSfcProduceNewPagedQuery pagedQuery);

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
        /// 根据SFCId获取数据
        /// </summary>
        /// <param name="sfcId"></param>
        /// <returns></returns>
        Task<ManuSfcProduceEntity> GetBySFCIdAsync(long sfcId);

        /// <summary>
        /// 根据SFCId获取数据
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetBySFCIdsAsync(IEnumerable<long> sfcIds);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetManuSfcProduceEntitiesAsync(ManuSfcProduceQuery query);

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
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeWithStatusCheckAsync(IEnumerable<ManuSfcProduceEntity>? entities);

        /// <summary>
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="multiUpdateStatusCommand"></param>
        /// <returns></returns>
        Task<int> UpdateProduceInStationSFCAsync(IEnumerable<UpdateProduceInStationSFCCommand> multiUpdateProduceInStationSFCCommands);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys);

        /// <summary>
        /// 批量更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateQtyRangeAsync(IEnumerable<UpdateSfcProcedureQtyByIdCommand> commands);

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
        /// 批量删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<int> DeletePhysicalRangeByIdsAsync(PhysicalDeleteSFCProduceByIdsCommand idsCommand);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeletePhysicalByIdSqlAsync(long id);

        /// <summary>
        /// 批量更新条码IsScrap
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateIsScrapAsync(UpdateIsScrapCommand command);

        /// <summary>
        /// 根据清空复投次数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> CleanRepeatedCountById(CleanRepeatedCountCommand command);

        /// <summary>
        /// 批量更新条码工艺路线和工序信息（条码独立更新）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateRouteByIdRangeAsync(IEnumerable<ManuSfcUpdateRouteByIdCommand> commands);

        /// <summary>
        /// 更新条码工艺路线和工序信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateRouteByIdAsync(ManuSfcUpdateRouteByIdCommand command);

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
        /// 更新条码Status
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateStatusByIdRangeAsync(IEnumerable<UpdateManuSfcProduceStatusByIdCommand> commands);

        /// <summary>
        /// 更新条码Status
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateStatusByIdAsync(UpdateManuSfcProduceStatusByIdCommand command);


        /// <summary>
        /// 更新资源，工艺路线，工序，维修状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateProcedureIdAsync(UpdateProcedureCommand command);

        /// <summary>
        /// 根据SFC批量更新工序与状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateProcedureAndStatusRangeAsync(UpdateProcedureAndStatusCommand command);


        /// <summary>
        /// 根据SFCs批量更新资源
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param 
        /// <returns></returns>
        Task<int> UpdateProcedureAndResourceRangeAsync(UpdateProcedureAndResourceCommand command);

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
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> entities);

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
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetSfcProduceBusinessEntitiesBySFCAsync(SfcListProduceBusinessQuery query);

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
        Task<int> DeleteSfcProduceBusinessBySfcInfoIdsAsync(DeleteSFCProduceBusinesssByIdsCommand command);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteSfcProduceBusinessBySfcInfoIdAsync(DeleteSfcProduceBusinesssBySfcInfoIdCommand command);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteSfcProduceBusinessByIdAsync(IEnumerable<long> ids);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="sfcInfoIds"></param>
        /// <returns></returns>
        Task<int> DeleteSfcProduceBusinesssAsync(DeleteSfcProduceBusinesssCommand command);

        /// <summary>
        /// 根据SFCs 获取数据
        /// </summary>
        /// <param name="sfcsQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetListBySfcsAsync(ManuSfcProduceBySfcsQuery sfcsQuery);

        /// <summary>
        /// 根据工序ID、资源ID,状态获取在制品数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcProduceEntity>> GetActivityListByProcedureIdStatusAsync(ManuSfcProduceByProcedureIdStatusQuery query);

        /// <summary>
        /// 分页查询（查询所有在制条码信息，加入载具）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcProduceVehicleView>> GetManuSfcPageListAsync(ManuSfcProduceVehiclePagedQuery query);

        /// <summary>
        /// 根据条码更改条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAndQtyBySfcsAsync(UpdateStatusAndQtyBySfcsCommand command);

        /// <summary>
        /// 根据id更改数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateQtyByIdAsync(UpdateManuSfcProduceQtyByIdCommand command);

        /// <summary>
        /// 部分报废 修改数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> PartialScrapManuSfcProduceByIdAsync(IEnumerable<ManuSfcProducePartialScrapByIdCommand> commands);
    }
}
