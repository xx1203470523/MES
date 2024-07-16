/*
 *creator: Karl
 *
 *describe: 物料台账    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
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
    /// 物料台账 service接口
    /// </summary>
    public interface IWhMaterialStandingbookService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialStandingbookDto>> GetPageListAsync(WhMaterialStandingbookPagedQueryDto whMaterialStandingbookPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialStandingbookCreateDto"></param>
        /// <returns></returns>
        Task CreateWhMaterialStandingbookAsync(WhMaterialStandingbookCreateDto whMaterialStandingbookCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialStandingbookModifyDto"></param>
        /// <returns></returns>
        Task ModifyWhMaterialStandingbookAsync(WhMaterialStandingbookModifyDto whMaterialStandingbookModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteWhMaterialStandingbookAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesWhMaterialStandingbookAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialStandingbookDto> QueryWhMaterialStandingbookByIdAsync(long id);

        /// <summary>
        /// 条码关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialStandingBookRelationDto>> GetWhMaterialStandingBookRelationByIdAsync(long id);

        /// <summary>
        /// 上料信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialStandingBookFeedingDto> GetWhMaterialStandingBookFeedingByIdAsync(long id);
    }
}
