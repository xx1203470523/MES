using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.MaskCode
{
    /// <summary>
    /// 服务接口（掩码维护）
    /// </summary>
    public interface IProcMaskCodeService
    {
        /// <summary>
        /// 添加（掩码维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ProcMaskCodeSaveDto createDto);

        /// <summary>
        /// 更新（掩码维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ProcMaskCodeSaveDto modifyDto);

        /// <summary>
        /// 删除（掩码维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 获取分页数据（掩码维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaskCodeDto>> GetPagedListAsync(ProcMaskCodePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（掩码维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaskCodeDto> GetDetailAsync(long id);
    }
}
