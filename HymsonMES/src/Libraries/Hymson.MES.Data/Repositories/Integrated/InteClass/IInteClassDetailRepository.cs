using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 班制维护明细仓储接口
    /// </summary>
    public interface IInteClassDetailRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteClassDetailEntity"></param>
        /// <returns></returns>
        Task InsertAsync(InteClassDetailEntity inteClassDetailEntity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<InteClassDetailEntity> entitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteClassDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteClassDetailEntity inteClassDetailEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<int> DeleteByClassIdAsync(long classId);

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
        Task<InteClassDetailEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteClassDetailEntity>> GetListByClassIdAsync(long classId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteClassDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteClassDetailEntity>> GetInteClassDetailEntitiesAsync(InteClassDetailQuery inteClassDetailQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteClassDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteClassDetailEntity>> GetPagedListAsync(InteClassDetailPagedQuery inteClassDetailPagedQuery);
    }
}
