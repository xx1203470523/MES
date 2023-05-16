/*
 *creator: Karl
 *
 *describe: 托盘信息仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteTray.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 托盘信息仓储接口
    /// </summary>
    public interface IInteTrayRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteTrayEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteTrayEntity inteTrayEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteTrayEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteTrayEntity> inteTrayEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteTrayEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteTrayEntity inteTrayEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteTrayEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteTrayEntity> inteTrayEntitys);

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
        Task<InteTrayEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteTrayEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteTrayQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteTrayEntity>> GetInteTrayEntitiesAsync(InteTrayQuery inteTrayQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteTrayPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteTrayEntity>> GetPagedInfoAsync(InteTrayPagedQuery inteTrayPagedQuery);
        #endregion
    }
}
