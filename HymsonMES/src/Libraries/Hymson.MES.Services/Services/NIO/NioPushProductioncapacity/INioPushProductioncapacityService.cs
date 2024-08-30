using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NIO;

namespace Hymson.MES.Services.Services.NIO
{
    /// <summary>
    /// 服务接口（合作伙伴精益与生产能力）
    /// </summary>
    public interface INioPushProductioncapacityService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(NioPushProductioncapacitySaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(NioPushProductioncapacitySaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NioPushProductioncapacityDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<NioPushProductioncapacityDto>> GetPagedListAsync(NioPushProductioncapacityPagedQueryDto pagedQueryDto);

    }
}