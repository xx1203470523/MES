using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（生产退料单明细）
    /// </summary>
    public interface IManuReturnOrderDetailService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuReturnOrderDetailSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuReturnOrderDetailSaveDto saveDto);

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
        Task<ManuReturnOrderDetailDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuReturnOrderDetailDto>> GetPagedListAsync(ManuReturnOrderDetailPagedQueryDto pagedQueryDto);

    }
}