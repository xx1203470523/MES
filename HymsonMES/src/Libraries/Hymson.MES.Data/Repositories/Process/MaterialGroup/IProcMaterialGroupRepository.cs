/*
 *creator: Karl
 *
 *describe: 物料组维护表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表仓储接口
    /// </summary>
    public interface IProcMaterialGroupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcMaterialGroupEntity procMaterialGroupEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcMaterialGroupEntity procMaterialGroupEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys);

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
        Task<ProcMaterialGroupEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialGroupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procMaterialGroupQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialGroupEntity>> GetProcMaterialGroupEntitiesAsync(ProcMaterialGroupQuery procMaterialGroupQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialGroupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialGroupEntity>> GetPagedInfoAsync(ProcMaterialGroupPagedQuery procMaterialGroupPagedQuery);
    }
}
