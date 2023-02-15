/*
 *creator: Karl
 *
 *describe: BOM明细替代料表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 05:33:28
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
    /// BOM明细替代料表仓储接口
    /// </summary>
    public interface IProcBomDetailReplaceMaterialRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys);

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
        Task<ProcBomDetailReplaceMaterialEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetProcBomDetailReplaceMaterialEntitiesAsync(ProcBomDetailReplaceMaterialQuery procBomDetailReplaceMaterialQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBomDetailReplaceMaterialEntity>> GetPagedInfoAsync(ProcBomDetailReplaceMaterialPagedQuery procBomDetailReplaceMaterialPagedQuery);
    }
}
