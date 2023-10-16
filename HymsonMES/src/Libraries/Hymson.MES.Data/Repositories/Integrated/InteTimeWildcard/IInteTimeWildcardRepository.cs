/*
 *creator: Karl
 *
 *describe: 时间通配（转换）仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-10-13 06:33:21
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 时间通配（转换）仓储接口
    /// </summary>
    public interface IInteTimeWildcardRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteTimeWildcardEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteTimeWildcardEntity inteTimeWildcardEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteTimeWildcardEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteTimeWildcardEntity> inteTimeWildcardEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteTimeWildcardEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteTimeWildcardEntity inteTimeWildcardEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteTimeWildcardEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteTimeWildcardEntity> inteTimeWildcardEntitys);

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
        Task<InteTimeWildcardEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteTimeWildcardEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteTimeWildcardQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteTimeWildcardEntity>> GetInteTimeWildcardEntitiesAsync(InteTimeWildcardQuery inteTimeWildcardQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteTimeWildcardPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteTimeWildcardEntity>> GetPagedInfoAsync(InteTimeWildcardPagedQuery inteTimeWildcardPagedQuery);


        /// <summary>
        /// 根据编码与类型获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteTimeWildcardEntity> GetByCodeAndTypeAsync(InteTimeWildcardCodeAndTypeQuery query);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteTimeWildcardEntity>> GetAllAsync(long siteId);
        #endregion
    }
}
