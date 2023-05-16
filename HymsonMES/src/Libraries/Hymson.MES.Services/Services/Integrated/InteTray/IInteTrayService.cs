/*
 *creator: Karl
 *
 *describe: 托盘信息    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 托盘信息 service接口
    /// </summary>
    public interface IInteTrayService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteTrayPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteTrayDto>> GetPagedListAsync(InteTrayPagedQueryDto inteTrayPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteTrayCreateDto"></param>
        /// <returns></returns>
        Task CreateInteTrayAsync(InteTrayCreateDto inteTrayCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteTrayModifyDto"></param>
        /// <returns></returns>
        Task ModifyInteTrayAsync(InteTrayModifyDto inteTrayModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteTrayAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteTrayAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteTrayDto> QueryInteTrayByIdAsync(long id);
    }
}
