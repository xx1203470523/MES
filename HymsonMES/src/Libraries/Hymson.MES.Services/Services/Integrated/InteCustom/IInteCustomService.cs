/*
 *creator: Karl
 *
 *describe: 客户维护    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 客户维护 service接口
    /// </summary>
    public interface IInteCustomService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteCustomPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCustomDto>> GetPagedListAsync(InteCustomPagedQueryDto inteCustomPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCustomCreateDto"></param>
        /// <returns></returns>
        Task CreateInteCustomAsync(InteCustomCreateDto inteCustomCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCustomModifyDto"></param>
        /// <returns></returns>
        Task ModifyInteCustomAsync(InteCustomModifyDto inteCustomModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteCustomAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteCustomAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCustomDto> QueryInteCustomByIdAsync(long id);

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入客户信息表格
        /// </summary>
        /// <returns></returns>
        Task ImportInteCustomAsync(IFormFile formFile);

    }
}
