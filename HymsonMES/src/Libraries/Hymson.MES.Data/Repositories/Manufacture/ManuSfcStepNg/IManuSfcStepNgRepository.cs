/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤ng信息记录表仓储接口
    /// </summary>
    public interface IManuSfcStepNgRepository
    {
        #region 方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepNgEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcStepNgEntity manuSfcStepNgEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepNgEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepNgEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcStepNgEntity manuSfcStepNgEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcStepNgEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys);

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
        Task<ManuSfcStepNgEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepNgEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据BarCodeStepId批量获取数据
        /// </summary>
        /// <param name="manuSfcStepIdsNgQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepNgEntity>> GetByBarCodeStepIdsAsync(ManuSfcStepIdsNgQuery manuSfcStepIdsNgQuery);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcStepNgQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepNgEntity>> GetManuSfcStepNgEntitiesAsync(ManuSfcStepNgQuery manuSfcStepNgQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepNgPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepNgEntity>> GetPagedInfoAsync(ManuSfcStepNgPagedQuery manuSfcStepNgPagedQuery);
        #endregion

        #region 扩展方法

        /// <summary>
        /// 联表分页查询
        /// </summary>
        /// <param name="manuSfcStepNgPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepNgEntity>> GetJoinPagedInfoAsync(ManuSfcStepNgPagedQuery manuSfcStepNgPagedQuery);

        #endregion
    }
}
