/*
 *creator: Karl
 *
 *describe: BOM表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM表仓储接口
    /// </summary>
    public interface IProcBomRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcBomEntity procBomEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcBomEntity> procBomEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcBomEntity procBomEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procBomEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcBomEntity> procBomEntitys);

        /// <summary>
        /// 更新 BOM IsCurrentVersion 为 false
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> UpdateIsCurrentVersionIsFalseAsync(long[] ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

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
        Task<ProcBomEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procBomQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomEntity>> GetProcBomEntitiesAsync(ProcBomQuery procBomQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomEntity>> GetEntitiesAsync(ProcBomQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcBomEntity>> GetPagedInfoAsync(ProcBomPagedQuery procBomPagedQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 根据编码获取Bom信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcBomEntity>> GetByCodesAsync(ProcBomsByCodeQuery param);

    }
}
