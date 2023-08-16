using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;

namespace Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository
{
    /// <summary>
    /// 作业表仓储
    /// @author admin
    /// @date 2023-02-21
    public interface IInteJobRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobEntity>> GetPagedInfoAsync(InteJobPagedQuery param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobEntity>> GetEntitiesAsync(EntityBySiteIdQuery query);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<InteJobEntity> GetByCodeAsync(EntityByCodeQuery param);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteJobEntity param);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertRangAsync(IEnumerable<InteJobEntity> param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteJobEntity param);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateRangAsync(IEnumerable<InteJobEntity> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeleteRangAsync(DeleteCommand param);
    }
}