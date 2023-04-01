/*
 *creator: Karl
 *
 *describe: 操作面板仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:05:24
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板仓储接口
    /// </summary>
    public interface IManuFacePlateRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateEntity manuFacePlateEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateEntity> manuFacePlateEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateEntity manuFacePlateEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateEntity> manuFacePlateEntitys);

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
        Task<ManuFacePlateEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateEntity>> GetManuFacePlateEntitiesAsync(ManuFacePlateQuery manuFacePlateQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlatePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateEntity>> GetPagedInfoAsync(ManuFacePlatePagedQuery manuFacePlatePagedQuery);
        #endregion
    }
}
