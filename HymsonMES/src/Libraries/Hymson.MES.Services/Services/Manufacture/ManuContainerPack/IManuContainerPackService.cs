/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） service接口
    /// </summary>
    public interface IManuContainerPackService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerPackDto>> GetPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerPackCreateDto"></param>
        /// <returns></returns>
        Task CreateManuContainerPackAsync(ManuContainerPackCreateDto manuContainerPackCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuContainerPackAsync(ManuContainerPackModifyDto manuContainerPackModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuContainerPackAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeletesManuContainerPackAsync(ManuContainerPackUnpackDto param);

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteAllByContainerBarCodeIdAsync(ContainerUnpackDto param);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuContainerPackDto> QueryManuContainerPackByIdAsync(long id);

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateContainerPackExJobDto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseDto>> ExecuteJobAsync(ManuFacePlateContainerPackExJobDto manuFacePlateContainerPackExJobDto);
    }
}
