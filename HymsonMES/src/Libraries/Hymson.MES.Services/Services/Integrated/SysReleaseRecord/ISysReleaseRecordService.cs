/*
 *creator: Karl
 *
 *describe: 发布记录表    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 发布记录表 service接口
    /// </summary>
    public interface ISysReleaseRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="sysReleaseRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<SysReleaseRecordDto>> GetPagedListAsync(SysReleaseRecordPagedQueryDto sysReleaseRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sysReleaseRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateSysReleaseRecordAsync(SysReleaseRecordCreateDto sysReleaseRecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysReleaseRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifySysReleaseRecordAsync(SysReleaseRecordModifyDto sysReleaseRecordModifyDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysReleaseRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifySysReleaseRecordStatusAsync(SysReleaseRecordModifyDto sysReleaseRecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteSysReleaseRecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesSysReleaseRecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysReleaseRecordDto> QuerySysReleaseRecordByIdAsync(long id);
    }
}
