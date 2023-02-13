/*
 *creator: Karl
 *
 *describe: 工序配置打印表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:24:06
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
    /// 工序配置打印表仓储接口
    /// </summary>
    public interface IProcProcedurePrintReleationRepository
    {
	    /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedurePrintReleationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedurePrintReleationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProcedurePrintReleationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedurePrintReleationEntity>> GetProcProcedurePrintReleationEntitiesAsync(ProcProcedurePrintReleationQuery procProcedurePrintReleationQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcedurePrintReleationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedurePrintReleationEntity>> GetPagedInfoAsync(ProcProcedurePrintReleationPagedQuery procProcedurePrintReleationPagedQuery);
		
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcedurePrintReleationEntity procProcedurePrintReleationEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ProcProcedurePrintReleationEntity> procProcedurePrintReleationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcedurePrintReleationEntity procProcedurePrintReleationEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcProcedurePrintReleationEntity> procProcedurePrintReleationEntitys);

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
