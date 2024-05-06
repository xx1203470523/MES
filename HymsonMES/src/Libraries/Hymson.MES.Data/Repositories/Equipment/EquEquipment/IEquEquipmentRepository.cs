using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentRepository
    {
        /// <summary>
        /// equipmentUnitEntity
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentEntity equipmentUnitEntity);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="equipmentEntities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquEquipmentEntity> equipmentEntities);

        /// <summary>
        /// equipmentUnitEntity
        /// </summary>
        /// <param name="equipmentUnitEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentEntity equipmentUnitEntity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equipmentEntities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquEquipmentEntity> equipmentEntities);

        /// <summary>
        /// 批量修改设备的设备组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateEquipmentGroupIdAsync(UpdateEquipmentGroupIdCommand command);

        /// <summary>
        /// 清空设备的设备组
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        Task<int> ClearEquipmentGroupIdAsync(long equipmentGroupId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetByIdAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(EquEquipmentGroupIdQuery param);

        /// <summary>
        /// 根据设备编码查询实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentEntity> GetByEquipmentCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 查询所有设备
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetBaseListAsync(EntityBySiteIdQuery query);

        /// <summary>
        /// equipmentQuery
        /// </summary>
        /// <param name="equipmentQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetEntitiesAsync(EquEquipmentQuery equipmentQuery);

        /// <summary>
        /// pagedQuery
        /// </summary>
        /// <param name="equipmentPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentPageView>> GetPagedListAsync(EquEquipmentPagedQuery pagedQuery);

        #region 顷刻项目

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentResAllView> GetEquResAllAsync(EquResAllQuery query);

        /// <summary>
        /// 查多个-根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentResAllView>> GetMultEquResAllAsync(MultEquResAllQuery query);

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentResAllView> GetEquResAsync(EquResAllQuery query);

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，线体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquEquipmentResAllView> GetEquResLineAsync(EquResAllQuery query);

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<EquEquipmentResAllView> GetEquResProcedureAsync(EquResAllQuery param);

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentEntity>> GetBySiteIdAsync(EquQuery query);

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> GetEquTokenSqlAsync(EquResAllQuery query);

        #endregion
    }
}
