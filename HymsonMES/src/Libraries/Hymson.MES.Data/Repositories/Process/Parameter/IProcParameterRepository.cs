/*
 *creator: Karl
 *
 *describe: 标准参数表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表仓储接口
    /// </summary>
    public interface IProcParameterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcParameterEntity procParameterEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procParameterEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcParameterEntity> procParameterEntitys);

        /// <summary>
        /// 忽略新增
        /// </summary>
        /// <param name="procParameterEntitys"></param>
        /// <returns></returns>
        Task<int> InsertIgnoresAsync(List<ProcParameterEntity> procParameterEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcParameterEntity procParameterEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procParameterEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcParameterEntity> procParameterEntitys);

        /// <summary>
        /// 删除
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
        Task<ProcParameterEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetByCodesAsync(EntityByCodesQuery query);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetAllAsync(long siteId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procParameterQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetProcParameterEntitiesAsync(ProcParameterQuery procParameterQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procParameterPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterEntity>> GetPagedInfoAsync(ProcParameterPagedQuery procParameterPagedQuery);
    }
}
