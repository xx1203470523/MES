/*
 *creator: Karl
 *
 *describe: 设备维修记录故障详情仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrderFault;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquRepairOrderFault
{
    /// <summary>
    /// 设备维修记录故障详情仓储接口
    /// </summary>
    public interface IEquRepairOrderFaultRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderFaultEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquRepairOrderFaultEntity equRepairOrderFaultEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderFaultEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquRepairOrderFaultEntity> equRepairOrderFaultEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderFaultEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquRepairOrderFaultEntity equRepairOrderFaultEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equRepairOrderFaultEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquRepairOrderFaultEntity> equRepairOrderFaultEntitys);

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
        Task<EquRepairOrderFaultEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderFaultEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equRepairOrderFaultQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderFaultEntity>> GetEquRepairOrderFaultEntitiesAsync(EquRepairOrderFaultQuery equRepairOrderFaultQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderFaultPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquRepairOrderFaultEntity>> GetPagedInfoAsync(EquRepairOrderFaultPagedQuery equRepairOrderFaultPagedQuery);
        #endregion
    }
}
