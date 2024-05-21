using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障类型仓储接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IEquipmentFaultTypeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentFaultTypeEntity param);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertRangAsync(List<EquEquipmentFaultTypeEntity> param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentFaultTypeEntity param);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateRangAsync(List<EquEquipmentFaultTypeEntity> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentFaultTypeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrintSetupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentFaultTypeEntity>> GetListByProcedureIdAsync(EQualUnqualifiedGroupQuery param);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentFaultTypeEntity>> GetPagedInfoAsync(EQualUnqualifiedGroupPagedQuery param);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<EquEquipmentFaultTypeEntity> GetByCodeAsync(QualUnqualifiedGroupByCodeQuery param);

        /// <summary>
        /// 插入不合格代码组关联不合格代码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedCodeGroupRelationRangAsync(List<EQualUnqualifiedCodeGroupRelation> param);

        /// <summary>
        /// 删除不合格代码组关联不合格代码
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long id);

        /// <summary>
        /// 删除不合格组关联不合格代码
        /// </summary>
        /// <param name="unqualifiedCodeId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedCodeGroupRelationByUnqualifiedIdAsync(long unqualifiedCodeId);

        /// <summary>
        /// 插入不合格组关联工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedGroupProcedureRelationRangAsync(List<EQualUnqualifiedGroupProcedureRelation> param);

        /// <summary>
        /// 删除不合格组关联工序
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long id);

        /// <summary>
        /// 获取设备故障类型关联故障现象关系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentFaultEquipmentGroupRelationView>> GetQualUnqualifiedCodeProcedureRelationAsync(long id);

        /// <summary>
        /// 获取设备故障类型关联设备现象关系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentFaultPhenomenonRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long id);
    }
}
