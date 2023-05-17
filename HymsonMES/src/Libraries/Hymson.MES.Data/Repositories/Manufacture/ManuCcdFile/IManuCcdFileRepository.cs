/*
 *creator: Karl
 *
 *describe: CCD文件仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-17 11:09:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// CCD文件仓储接口
    /// </summary>
    public interface IManuCcdFileRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuCcdFileEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuCcdFileEntity manuCcdFileEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuCcdFileEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuCcdFileEntity> manuCcdFileEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuCcdFileEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuCcdFileEntity manuCcdFileEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuCcdFileEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuCcdFileEntity> manuCcdFileEntitys);

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
        Task<ManuCcdFileEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuCcdFileEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuCcdFileQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuCcdFileEntity>> GetManuCcdFileEntitiesAsync(ManuCcdFileQuery manuCcdFileQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuCcdFilePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuCcdFileEntity>> GetPagedInfoAsync(ManuCcdFilePagedQuery manuCcdFilePagedQuery);
        #endregion
    }
}
