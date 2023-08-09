using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProcProcessEquipmentGroupRelation.Query;
using Hymson.MES.Data.Repositories.Process.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储接口（设备组关联设备表）
    /// </summary>
    public interface IProcProcessEquipmentGroupRelationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcessEquipmentGroupRelationEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcProcessEquipmentGroupRelationEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcessEquipmentGroupRelationEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ProcProcessEquipmentGroupRelationEntity> entities);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);



        /// <summary>
        /// 通过ProcEquipmentGroupId删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByProcEquIdAsync(long equipmentGroupId);

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
        Task<ProcProcessEquipmentGroupRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processEquipmentGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetByGroupIdAsync(ProcProcessEquipmentGroupIdQuery param);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetEntitiesAsync(long SiteId);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessEquipmentGroupRelationEntity>> GetPagedInfoAsync(ProcProcessEquipmentGroupRelationPagedQuery pagedQuery);

    }
}
