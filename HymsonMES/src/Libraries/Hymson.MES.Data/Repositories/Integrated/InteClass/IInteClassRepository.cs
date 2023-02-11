using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 生产班次仓储接口
    /// </summary>
    public interface IInteClassRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteClassEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteClassEntity inteClassEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteClassEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteClassEntity inteClassEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

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
        /// <param name="inteClassPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteClassEntity>> GetPagedListAsync(InteClassPagedQuery inteClassPagedQuery);
    }
}
