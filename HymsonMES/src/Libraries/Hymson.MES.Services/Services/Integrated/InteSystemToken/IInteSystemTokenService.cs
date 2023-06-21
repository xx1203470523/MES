/*
 *creator: Karl
 *
 *describe: 系统Token    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 系统Token service接口
    /// </summary>
    public interface IInteSystemTokenService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteSystemTokenPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteSystemTokenDto>> GetPagedListAsync(InteSystemTokenPagedQueryDto inteSystemTokenPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteSystemTokenCreateDto"></param>
        /// <returns></returns>
        Task CreateInteSystemTokenAsync(InteSystemTokenCreateDto inteSystemTokenCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteSystemTokenModifyDto"></param>
        /// <returns></returns>
        Task ModifyInteSystemTokenAsync(InteSystemTokenModifyDto inteSystemTokenModifyDto);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        Task<string> RefreshSystemTokenAsync(long systemId);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteSystemTokenAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteSystemTokenDto> QueryInteSystemTokenByIdAsync(long id);
    }
}
