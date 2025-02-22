/*
 *creator: Karl
 *
 *describe: 烘烤工序仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 烘烤工序仓储接口
    /// </summary>
    public interface IManuBakingRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuBakingEntity manuBakingEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuBakingEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuBakingEntity> manuBakingEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuBakingEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuBakingEntity manuBakingEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuBakingEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuBakingEntity> manuBakingEntitys);

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
        Task<ManuBakingEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBakingEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuBakingQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuBakingEntity>> GetManuBakingEntitiesAsync(ManuBakingQuery manuBakingQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuBakingPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBakingEntity>> GetPagedInfoAsync(ManuBakingPagedQuery manuBakingPagedQuery);
        #endregion
    }
}
