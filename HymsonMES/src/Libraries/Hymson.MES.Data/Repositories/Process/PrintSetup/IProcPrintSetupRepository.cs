/*
 *creator: Karl
 *
 *describe: 打印设置表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 打印设置表接口
    /// </summary>
    public interface IProcPrintSetupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procConversionFactorEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcPrintSetupEntity procConversionFactorEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procPrintSetupEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcPrintSetupEntity procPrintSetupEntity);

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
        Task<ProcPrintSetupEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据materialId获取数据
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<ProcPrintSetupEntity> GetByMaterialIdAsync(long materialId);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrintSetupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcLoadPointEntity>> GetByResourceIdAsync(long resourceId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procPrintSetupQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcPrintSetupEntity>> GetProcConversionFactorEntitiesAsync(ProcPrintSetupQuery procPrintSetupQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procPrintSetupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrintSetupView>> GetPagedInfoAsync(IProcPrintSetupPagedQuery procLoadPointPagedQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

    }
}
