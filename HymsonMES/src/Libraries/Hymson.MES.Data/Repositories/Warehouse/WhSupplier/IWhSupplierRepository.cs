/*
 *creator: Karl
 *
 *describe: 供应商仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 供应商仓储接口
    /// </summary>
    public interface IWhSupplierRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whSupplierEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhSupplierEntity whSupplierEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whSupplierEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<WhSupplierEntity> whSupplierEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whSupplierEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhSupplierEntity whSupplierEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="whSupplierEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<WhSupplierEntity> whSupplierEntitys);

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
        Task<WhSupplierEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhSupplierEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="whSupplierQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<WhSupplierEntity>> GetWhSupplierEntitiesAsync(WhSupplierQuery whSupplierQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whSupplierPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhSupplierEntity>> GetPagedInfoAsync(WhSupplierPagedQuery whSupplierPagedQuery);
    }
}
