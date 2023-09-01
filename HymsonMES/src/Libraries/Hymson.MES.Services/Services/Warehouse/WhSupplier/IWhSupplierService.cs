/*
 *creator: Karl
 *
 *describe: 供应商    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 供应商 service接口
    /// </summary>
    public interface IWhSupplierService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="whSupplierPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhSupplierDto>> GetPageListAsync(WhSupplierPagedQueryDto whSupplierPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whSupplierCreateDto"></param>
        /// <returns></returns>
        Task CreateWhSupplierAsync(WhSupplierCreateDto whSupplierCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whSupplierModifyDto"></param>
        /// <returns></returns>
        Task ModifyWhSupplierAsync(WhSupplierModifyDto whSupplierModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteWhSupplierAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesWhSupplierAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhSupplierDto> QueryWhSupplierByIdAsync(long id);

        /// <summary>
        /// 根据ID查询(更改供应商)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UpdateWhSupplierDto> QueryUpdateWhSupplierByIdAsync(long id);
    }
}
