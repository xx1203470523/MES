using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 客户维护仓储接口
    /// </summary>
    public interface IInteCustomRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCustomEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteCustomEntity inteCustomEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCustomEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteCustomEntity> inteCustomEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCustomEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCustomEntity inteCustomEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteCustomEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteCustomEntity> inteCustomEntitys);

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
        /// 根据Code获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<InteCustomEntity> GetByCodeAsync(string code);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCustomEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteCustomQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomEntity>> GetInteCustomEntitiesAsync(InteCustomQuery inteCustomQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCustomEntity>> GetEntitiesAsync(InteCustomQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCustomEntity>> GetPagedInfoAsync(InteCustomPagedQuery query);
        #endregion
    }
}
