/*
 *creator: Karl
 *
 *describe: 产出上报仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-19 10:44:12
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产出上报仓储接口
    /// </summary>
    public interface IManuOutputRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuOutputEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuOutputEntity manuOutputEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuOutputEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuOutputEntity> manuOutputEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuOutputEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuOutputEntity manuOutputEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuOutputEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuOutputEntity> manuOutputEntitys);

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
        Task<ManuOutputEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuOutputEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuOutputQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuOutputEntity>> GetManuOutputEntitiesAsync(ManuOutputQuery manuOutputQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuOutputPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuOutputEntity>> GetPagedInfoAsync(ManuOutputPagedQuery manuOutputPagedQuery);
        #endregion
    }
}
