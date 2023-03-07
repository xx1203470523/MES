/*
 *creator: Karl
 *
 *describe: 物料库存    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
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
    /// 物料库存 service接口
    /// </summary>
    public interface IWhMaterialInventoryService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="whMaterialInventoryPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryDto>> GetPageListAsync(WhMaterialInventoryPagedQueryDto whMaterialInventoryPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryDto"></param>
        /// <returns></returns>
        Task CreateWhMaterialInventoryAsync(WhMaterialInventoryCreateDto whMaterialInventoryCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialInventoryDto"></param>
        /// <returns></returns>
        Task ModifyWhMaterialInventoryAsync(WhMaterialInventoryModifyDto whMaterialInventoryModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteWhMaterialInventoryAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesWhMaterialInventoryAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryDto> QueryWhMaterialInventoryByIdAsync(long id);
    }
}
