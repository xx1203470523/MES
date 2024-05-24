/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系仓储接口
    /// </summary>
    public interface IEquSpotcheckTemplateEquipmentGroupRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSpotcheckTemplateEquipmentGroupRelationEntity equSpotcheckTemplateEquipmentGroupRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSpotcheckTemplateEquipmentGroupRelationEntity> equSpotcheckTemplateEquipmentGroupRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSpotcheckTemplateEquipmentGroupRelationEntity equSpotcheckTemplateEquipmentGroupRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSpotcheckTemplateEquipmentGroupRelationEntity> equSpotcheckTemplateEquipmentGroupRelationEntitys);

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
        Task<int> DeletesBySpotCheckTemplateIdsAsync(IEnumerable<long> spotCheckTemplateIds);

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<int> DeletesByTemplateIdAndGroupIdsAsync(GetByTemplateIdAndGroupIdQuery spotCheckTemplateIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckTemplateEquipmentGroupRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据IDs批量获取数据(组合)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByTemplateIdAndGroupIdAsync(GetByTemplateIdAndGroupIdQuery param);


        /// <summary>
        /// 根据GroupId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByGroupIdAsync(IEnumerable<long> groupIdSql);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetEquSpotcheckTemplateEquipmentGroupRelationEntitiesAsync(EquSpotcheckTemplateEquipmentGroupRelationQuery equSpotcheckTemplateEquipmentGroupRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetPagedInfoAsync(EquSpotcheckTemplateEquipmentGroupRelationPagedQuery equSpotcheckTemplateEquipmentGroupRelationPagedQuery);
        #endregion
    }
}
