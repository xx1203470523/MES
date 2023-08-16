/*
 *creator: Karl
 *
 *describe: 设备验证仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-28 09:02:39
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备验证仓储接口
    /// </summary>
    public interface IEquEquipmentVerifyRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentVerifyEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentVerifyEntity equEquipmentVerifyEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquEquipmentVerifyEntity> equEquipmentVerifyEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentVerifyEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentVerifyEntity equEquipmentVerifyEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equEquipmentVerifyEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquEquipmentVerifyEntity> equEquipmentVerifyEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除 (真删除)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentVerifyEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentVerifyEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equEquipmentVerifyQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentVerifyEntity>> GetEquEquipmentVerifyEntitiesAsync(EquEquipmentVerifyQuery equEquipmentVerifyQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentVerifyPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentVerifyEntity>> GetPagedInfoAsync(EquEquipmentVerifyPagedQuery equEquipmentVerifyPagedQuery);
        #endregion

        /// <summary>
        /// 根据设备ID批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesByEquipmentIdsAsync(long[] equipmentIds);

        /// <summary>
        /// 根据设备ID查询对应的验证
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentVerifyEntity>> GetEquipmentVerifyByEquipmentIdAsync(long equipmentId);
    }
}
