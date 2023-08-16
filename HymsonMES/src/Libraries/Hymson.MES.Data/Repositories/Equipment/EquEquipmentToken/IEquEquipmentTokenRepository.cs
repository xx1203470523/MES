/*
 *creator: Karl
 *
 *describe: 设备Token仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-06-07 02:17:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备Token仓储接口
    /// </summary>
    public interface IEquEquipmentTokenRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentTokenEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentTokenEntity equEquipmentTokenEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentTokenEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquEquipmentTokenEntity> equEquipmentTokenEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentTokenEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentTokenEntity equEquipmentTokenEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equEquipmentTokenEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquEquipmentTokenEntity> equEquipmentTokenEntitys);

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
        /// 根据EquipmentId获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentTokenEntity> GetByEquipmentIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentTokenEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentTokenEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equEquipmentTokenQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentTokenEntity>> GetEquEquipmentTokenEntitiesAsync(EquEquipmentTokenQuery equEquipmentTokenQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentTokenPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentTokenEntity>> GetPagedInfoAsync(EquEquipmentTokenPagedQuery equEquipmentTokenPagedQuery);
        #endregion
    }
}
