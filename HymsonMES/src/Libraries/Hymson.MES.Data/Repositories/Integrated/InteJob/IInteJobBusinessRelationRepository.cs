/*
 *creator: Karl
 *
 *describe: job业务配置配置表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
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
    /// job业务配置配置表仓储接口
    /// </summary>
    public interface IInteJobBusinessRelationRepository
    {
	    /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobBusinessRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteJobBusinessRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetInteJobBusinessRelationEntitiesAsync(InteJobBusinessRelationQuery inteJobBusinessRelationQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteJobBusinessRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobBusinessRelationEntity>> GetPagedInfoAsync(InteJobBusinessRelationPagedQuery inteJobBusinessRelationPagedQuery);
		
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByBusinessIdAsync(long id);
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(long[] ids);
    }
}
