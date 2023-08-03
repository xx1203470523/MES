/*
 *creator: Karl
 *
 *describe: 设备参数组仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组仓储接口
    /// </summary>
    public interface IProcEquipmentGroupParamRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys);

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
        Task<ProcEquipmentGroupParamEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procEquipmentGroupParamQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetProcEquipmentGroupParamEntitiesAsync(ProcEquipmentGroupParamQuery procEquipmentGroupParamQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEquipmentGroupParamPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEquipmentGroupParamEntity>> GetPagedInfoAsync(ProcEquipmentGroupParamPagedQuery procEquipmentGroupParamPagedQuery);
        #endregion
    }
}
