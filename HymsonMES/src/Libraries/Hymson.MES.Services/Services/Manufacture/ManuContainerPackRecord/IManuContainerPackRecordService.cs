/*
 *creator: Karl
 *
 *describe: 容器装载记录    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器装载记录 service接口
    /// </summary>
    public interface IManuContainerPackRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuContainerPackRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerPackRecordDto>> GetPagedListAsync(ManuContainerPackRecordPagedQueryDto manuContainerPackRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuContainerPackRecordAsync(ManuContainerPackRecordCreateDto manuContainerPackRecordCreateDto);
        Task CreateManuContainerPackRecordsAsync(List<ManuContainerPackRecordCreateDto> manuContainerPackRecordCreateDtos);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuContainerPackRecordAsync(ManuContainerPackRecordModifyDto manuContainerPackRecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuContainerPackRecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuContainerPackRecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuContainerPackRecordDto> QueryManuContainerPackRecordByIdAsync(long id);
    }
}
