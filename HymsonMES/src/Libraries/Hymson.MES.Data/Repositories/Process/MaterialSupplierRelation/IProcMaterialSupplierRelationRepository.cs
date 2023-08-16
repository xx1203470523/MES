/*
 *creator: Karl
 *
 *describe: 物料供应商关系仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-27 02:30:48
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
    /// 物料供应商关系仓储接口
    /// </summary>
    public interface IProcMaterialSupplierRelationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcMaterialSupplierRelationEntity procMaterialSupplierRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcMaterialSupplierRelationEntity> procMaterialSupplierRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcMaterialSupplierRelationEntity procMaterialSupplierRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procMaterialSupplierRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcMaterialSupplierRelationEntity> procMaterialSupplierRelationEntitys);

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
        /// 根据物料Id批量删除 (真删除)
        /// </summary>
        /// <param name="materialIds"></param>
        /// <returns></returns>
        Task<int> DeleteTrueByMaterialIdsAsync(long[] materialIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialSupplierRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 通过物料Id查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialSupplierView>> GetByMaterialIdAsync(long materialId);

        /// <summary>
        /// 通过供应商Id查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns> 
        Task<IEnumerable<ProcMaterialSupplierView>> GetBySupplierIdsAsync(long[] supplierIds);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialSupplierRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procMaterialSupplierRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialSupplierRelationEntity>> GetProcMaterialSupplierRelationEntitiesAsync(ProcMaterialSupplierRelationQuery procMaterialSupplierRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialSupplierRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialSupplierRelationEntity>> GetPagedInfoAsync(ProcMaterialSupplierRelationPagedQuery procMaterialSupplierRelationPagedQuery);
    }
}
