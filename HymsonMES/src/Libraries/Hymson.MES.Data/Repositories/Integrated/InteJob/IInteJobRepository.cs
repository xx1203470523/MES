/*
 *creator: Karl
 *
 *describe: 作业表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 作业表仓储接口
    /// </summary>
    public interface IInteJobRepository
    {
	    /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteJobQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobEntity>> GetInteJobEntitiesAsync(InteJobQuery inteJobQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteJobPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobEntity>> GetPagedInfoAsync(InteJobPagedQuery inteJobPagedQuery);
		
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteJobEntity inteJobEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<InteJobEntity> inteJobEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteJobEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteJobEntity inteJobEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteJobEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<InteJobEntity> inteJobEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(long[] ids);
    }
}
