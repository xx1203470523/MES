/*
 *creator: Karl
 *
 *describe: 工序配置作业表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:23:23
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
    /// 工序配置作业表仓储接口
    /// </summary>
    public interface IProcProcedureJobReleationRepository
    {

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcedureJobReleationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureJobReleationEntity>> GetPagedInfoAsync(ProcProcedureJobReleationPagedQuery procProcedureJobReleationPagedQuery);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureJobReleationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureJobReleationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProcedureJobReleationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureJobReleationEntity>> GetProcProcedureJobReleationEntitiesAsync(ProcProcedureJobReleationQuery procProcedureJobReleationQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureJobReleationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcedureJobReleationEntity procProcedureJobReleationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcedureJobReleationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ProcProcedureJobReleationEntity> procProcedureJobReleationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedureJobReleationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcedureJobReleationEntity procProcedureJobReleationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProcedureJobReleationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcProcedureJobReleationEntity> procProcedureJobReleationEntitys);

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
