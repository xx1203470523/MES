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
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentFaultTypeEntity param);

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
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentFaultTypeEntity>> GetPagedInfoAsync(EquipmentFaultTypePagedQuery param);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<EquEquipmentFaultTypeEntity> GetByCodeAsync(QualUnqualifiedGroupByCodeQuery param);

        /// <summary>
        /// 插入设备故障类型与现象关系
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedCodeGroupRelationRangAsync(List<EquipmentFaultTypesPhenomenonRelation> param);

        /// <summary>
        /// 删除设备故障类型关联现象
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long id);

        /// <summary>
        /// 插入设备故障类型关联设备组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertQualUnqualifiedGroupProcedureRelationRangAsync(List<EQualUnqualifiedGroupProcedureRelation> param);

        /// <summary>
        /// 删除设备故障类型关联设备组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long id);

        /// <summary>
        /// 获取设备故障类型关联设备组
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
