/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码仓储接口
    /// </summary>
    public interface IManuSfcStepMaterialRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcStepMaterialEntity manuSfcStepMaterialEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcStepMaterialEntity manuSfcStepMaterialEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcStepMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntitys);

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
        Task<ManuSfcStepMaterialEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepMaterialEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcStepMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepMaterialEntity>> GetManuSfcStepMaterialEntitiesAsync(ManuSfcStepMaterialQuery manuSfcStepMaterialQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepMaterialEntity>> GetPagedInfoAsync(ManuSfcStepMaterialPagedQuery manuSfcStepMaterialPagedQuery);
        #endregion
    }
}
