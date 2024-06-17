/*
 *creator: Karl
 *
 *describe: 设备台账信息仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息仓储接口
    /// </summary>
    public interface IEquEquipmentRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquEquipmentRecordEntity equEquipmentRecordEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquEquipmentRecordEntity> equEquipmentRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquEquipmentRecordEntity equEquipmentRecordEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equEquipmentRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquEquipmentRecordEntity> equEquipmentRecordEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentRecordEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equEquipmentRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquEquipmentRecordEntity>> GetEquEquipmentRecordEntitiesAsync(EquEquipmentRecordQuery equEquipmentRecordQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentRecordEntity>> GetPagedInfoAsync(EquEquipmentRecordPagedQuery equEquipmentRecordPagedQuery);
        #endregion
    }
}
