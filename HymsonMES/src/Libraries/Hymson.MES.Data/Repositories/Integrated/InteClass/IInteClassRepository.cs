using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 班制维护仓储接口
    /// </summary>
    public interface IInteClassRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteClassEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteClassEntity entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteClassEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteClassQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteClassEntity>> GetInteClassEntitiesAsync(InteClassQuery inteClassQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteClassEntity>> GetPagedListAsync(InteClassPagedQuery pagedQuery);
    }
}
