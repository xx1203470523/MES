/*
 *creator: Karl
 *
 *describe: 设备点检计划与设备关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 03:51:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSpotcheckPlanEquipmentRelation
{
    /// <summary>
    /// 设备点检计划与设备关系仓储接口
    /// </summary>
    public interface IEquSpotcheckPlanEquipmentRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationEntitys);

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
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> PhysicalDeletesAsync(IEnumerable<long> spotCheckPlanIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckPlanEquipmentRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据SpotCheckPlanId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetBySpotCheckPlanIdsAsync(long spotCheckPlanId);

        /// <summary>
        /// 根据SpotCheckTemplateIds批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetBySpotCheckSpotCheckTemplateIdsAsync(IEnumerable<long> spotCheckTemplateIds);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetEquSpotcheckPlanEquipmentRelationEntitiesAsync(EquSpotcheckPlanEquipmentRelationQuery equSpotcheckPlanEquipmentRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckPlanEquipmentRelationEntity>> GetPagedInfoAsync(EquSpotcheckPlanEquipmentRelationPagedQuery equSpotcheckPlanEquipmentRelationPagedQuery);
        #endregion
    }
}
