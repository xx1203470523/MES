using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated.InteJobClass
{
    /// <summary>
    /// 生产作业程序仓储接口
    /// </summary>
    public interface IInteJobClassRepository
    {
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobClassEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteJobClassEntity> inteJobClassEntitys);

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
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobClassEntity>> GetByIdsAsync(long[] ids);

    }
}
