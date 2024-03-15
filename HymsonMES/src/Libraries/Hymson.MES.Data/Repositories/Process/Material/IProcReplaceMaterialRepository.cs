/*
 *creator: Karl
 *
 *describe: 物料替代组件表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-09 11:28:39
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
    /// 物料替代组件表仓储接口
    /// </summary>
    public interface IProcReplaceMaterialRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity);

        Task<int> InsertsAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity);

        /// <summary>
        /// 批量更新-只更新 UpdatedOn,  UpdatedBy,  ReplaceMaterialId,  IsUse 
        /// </summary>
        /// <param name="procReplaceMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys);

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
        /// 通过主物料ID批量删除 （硬删除）
        /// </summary>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        Task<int> DeleteTrueByMaterialIdsAsync(long[] materialIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcReplaceMaterialEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据物料id查询替代料
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialEntity>> GetByMaterialIdAsync(long materialId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialEntity>> GetProcReplaceMaterialEntitiesAsync(ProcReplaceMaterialQuery procReplaceMaterialQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(ProcReplaceMaterialQuery procReplaceMaterialQuery);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(long siteId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewListAsync(long siteId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(ProcReplaceMaterialsQuery procReplaceMaterialQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcReplaceMaterialEntity>> GetPagedInfoAsync(ProcReplaceMaterialPagedQuery procReplaceMaterialPagedQuery);

        #region 顷刻

        /// <summary>
        /// 多个-根据物料id查询替代料
        /// </summary>
        /// <param name="materialIdList"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcReplaceMaterialEntity>> GetListByMaterialIdAsync(List<long> materialIdList);

        #endregion

    }
}
