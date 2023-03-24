/*
 *creator: pengxin
 *
 *describe: 设备故障原因表仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表仓储接口
    /// </summary>
    public interface IEquFaultReasonRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquFaultReasonEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquFaultReasonEntity EquFaultReasonEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquFaultReasonEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquFaultReasonEntity> EquFaultReasonEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquFaultReasonEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquFaultReasonEntity EquFaultReasonEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="EquFaultReasonEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquFaultReasonEntity> EquFaultReasonEntitys);

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
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="EquFaultReasonQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultReasonEntity>> GetEquFaultReasonEntitiesAsync(EquFaultReasonQuery EquFaultReasonQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquFaultReasonPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonEntity>> GetPagedInfoAsync(EquFaultReasonPagedQuery EquFaultReasonPagedQuery);
    }
}
