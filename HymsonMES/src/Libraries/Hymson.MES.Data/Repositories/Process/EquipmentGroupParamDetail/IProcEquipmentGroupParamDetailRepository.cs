/*
 *creator: Karl
 *
 *describe: 设备参数组详情仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 02:08:48
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组详情仓储接口
    /// </summary>
    public interface IProcEquipmentGroupParamDetailRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcEquipmentGroupParamDetailEntity procEquipmentGroupParamDetailEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcEquipmentGroupParamDetailEntity> procEquipmentGroupParamDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcEquipmentGroupParamDetailEntity procEquipmentGroupParamDetailEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcEquipmentGroupParamDetailEntity> procEquipmentGroupParamDetailEntitys);

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
        Task<ProcEquipmentGroupParamDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamDetailEntity>> GetProcEquipmentGroupParamDetailEntitiesAsync(ProcEquipmentGroupParamDetailQuery procEquipmentGroupParamDetailQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEquipmentGroupParamDetailEntity>> GetPagedInfoAsync(ProcEquipmentGroupParamDetailPagedQuery procEquipmentGroupParamDetailPagedQuery);
        #endregion
    }
}
