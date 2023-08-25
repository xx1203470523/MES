using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.InteContainer
{
    /// <summary>
    /// 服务接口（容器维护）
    /// </summary>
    public interface IInteContainerService
    {
        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(InteContainerSaveDto createDto);

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteContainerSaveDto modifyDto);

        /// <summary>
        /// 删除（容器维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 获取分页数据（容器维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteContainerDto>> GetPagedListAsync(InteContainerPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteContainerDto> GetDetailAsync(long id);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

    }
}
