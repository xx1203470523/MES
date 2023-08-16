/*
 *creator: Karl
 *
 *describe: 烘烤工序    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:41:12
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 烘烤工序 service接口
    /// </summary>
    public interface IManuBakingService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuBakingPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBakingDto>> GetPagedListAsync(ManuBakingPagedQueryDto manuBakingPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingCreateDto"></param>
        /// <returns></returns>
        Task CreateManuBakingAsync(ManuBakingCreateDto manuBakingCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuBakingModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuBakingAsync(ManuBakingModifyDto manuBakingModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuBakingAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuBakingAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuBakingDto> QueryManuBakingByIdAsync(long id);
    }
}
