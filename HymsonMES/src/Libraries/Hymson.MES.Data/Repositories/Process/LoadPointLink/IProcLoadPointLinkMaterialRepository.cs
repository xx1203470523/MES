/*
 *creator: Karl
 *
 *describe: 上料点关联物料表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:31:10
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
    /// 上料点关联物料表仓储接口
    /// </summary>
    public interface IProcLoadPointLinkMaterialRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcLoadPointLinkMaterialEntity procLoadPointLinkMaterialEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcLoadPointLinkMaterialEntity> procLoadPointLinkMaterialEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcLoadPointLinkMaterialEntity procLoadPointLinkMaterialEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcLoadPointLinkMaterialEntity> procLoadPointLinkMaterialEntitys);

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
        /// 根据LoadPointId批量真删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesByLoadPointIdTrueAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcLoadPointLinkMaterialEntity> GetByIdAsync(long id);    

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkMaterialEntity>> GetByIdsAsync(long[] ids);
        
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkMaterialView>> GetLoadPointLinkMaterialAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointLinkMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointLinkMaterialEntity>> GetProcLoadPointLinkMaterialEntitiesAsync(ProcLoadPointLinkMaterialQuery procLoadPointLinkMaterialQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointLinkMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcLoadPointLinkMaterialEntity>> GetPagedInfoAsync(ProcLoadPointLinkMaterialPagedQuery procLoadPointLinkMaterialPagedQuery);
    }
}
