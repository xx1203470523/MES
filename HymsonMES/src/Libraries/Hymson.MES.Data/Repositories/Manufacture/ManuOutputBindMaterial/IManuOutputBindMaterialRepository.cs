/*
 *creator: Karl
 *
 *describe: 产出上报绑定物料仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-19 10:46:49
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产出上报绑定物料仓储接口
    /// </summary>
    public interface IManuOutputBindMaterialRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuOutputBindMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuOutputBindMaterialEntity manuOutputBindMaterialEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuOutputBindMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuOutputBindMaterialEntity> manuOutputBindMaterialEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuOutputBindMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuOutputBindMaterialEntity manuOutputBindMaterialEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuOutputBindMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuOutputBindMaterialEntity> manuOutputBindMaterialEntitys);

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
        Task<ManuOutputBindMaterialEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuOutputBindMaterialEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuOutputBindMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuOutputBindMaterialEntity>> GetManuOutputBindMaterialEntitiesAsync(ManuOutputBindMaterialQuery manuOutputBindMaterialQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuOutputBindMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuOutputBindMaterialEntity>> GetPagedInfoAsync(ManuOutputBindMaterialPagedQuery manuOutputBindMaterialPagedQuery);
        #endregion
    }
}
