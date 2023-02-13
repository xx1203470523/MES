/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数关联类型表仓储接口
    /// </summary>
    public interface IProcParameterLinkTypeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterLinkTypeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcParameterLinkTypeEntity procParameterLinkTypeEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procParameterLinkTypeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcParameterLinkTypeEntity> procParameterLinkTypeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procParameterLinkTypeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcParameterLinkTypeEntity procParameterLinkTypeEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procParameterLinkTypeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcParameterLinkTypeEntity> procParameterLinkTypeEntitys);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcParameterLinkTypeEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterLinkTypeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procParameterLinkTypeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterLinkTypeEntity>> GetProcParameterLinkTypeEntitiesAsync(ProcParameterLinkTypeQuery procParameterLinkTypeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procParameterLinkTypePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcParameterLinkTypeEntity>> GetPagedInfoAsync(ProcParameterLinkTypePagedQuery procParameterLinkTypePagedQuery);
    }
}
