using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;

namespace Hymson.MES.Services.Services.WhMaterialReceiptDetail
{
    /// <summary>
    /// 服务接口（收料单详情）
    /// </summary>
    public interface IWhMaterialReceiptDetailService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(WHMaterialReceiptDetailSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WHMaterialReceiptDetailSaveDto saveDto);

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
        Task<WHMaterialReceiptDetailDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WHMaterialReceiptDetailDto>> GetPagedListAsync(WHMaterialReceiptDetailPagedQueryDto pagedQueryDto);

    }
}