/*
 *creator: Karl
 *
 *describe: BOM明细表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
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
    /// BOM明细表仓储接口
    /// </summary>
    public interface IProcBomDetailRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBomDetailEntity procBomDetailEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcBomDetailEntity> procBomDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBomDetailEntity procBomDetailEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBomDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcBomDetailEntity> procBomDetailEntitys);

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
        /// 批量删除关联的BomId的数据
        /// </summary>
        /// <param name="bomIds"></param>
        /// <returns></returns>
        Task<int> DeleteBomIDAsync(long[] bomIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcBomDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 查询主物料表列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailView>> GetListMainAsync(long id);

        /// <summary>
        /// 查询替代物料列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailView>> GetListReplaceAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBomDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomDetailEntity>> GetProcBomDetailEntitiesAsync(ProcBomDetailQuery procBomDetailQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBomDetailEntity>> GetPagedInfoAsync(ProcBomDetailPagedQuery procBomDetailPagedQuery);
    }
}
