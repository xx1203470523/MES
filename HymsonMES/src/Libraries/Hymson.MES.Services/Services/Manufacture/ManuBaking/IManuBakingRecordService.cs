/*
 *creator: Karl
 *
 *describe: 烘烤执行表    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 烘烤执行表 service接口
    /// </summary>
    public interface IManuBakingRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuBakingRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuBakingRecordDto>> GetPagedListAsync(ManuBakingRecordPagedQueryDto manuBakingRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuBakingRecordAsync(ManuBakingRecordCreateDto manuBakingRecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuBakingRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuBakingRecordAsync(ManuBakingRecordModifyDto manuBakingRecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuBakingRecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuBakingRecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuBakingRecordDto> QueryManuBakingRecordByIdAsync(long id);
    }
}
