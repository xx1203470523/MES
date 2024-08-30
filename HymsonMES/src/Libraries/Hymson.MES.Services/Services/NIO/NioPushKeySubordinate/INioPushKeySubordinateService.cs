using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NIO;

namespace Hymson.MES.Services.Services.NIO
{
    /// <summary>
    /// 服务接口（物料及其关键下级件信息表）
    /// </summary>
    public interface INioPushKeySubordinateService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(NioPushKeySubordinateSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(NioPushKeySubordinateSaveDto saveDto);

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
        Task<NioPushKeySubordinateDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<NioPushKeySubordinateDto>> GetPagedListAsync(NioPushKeySubordinatePagedQueryDto pagedQueryDto);

    }
}