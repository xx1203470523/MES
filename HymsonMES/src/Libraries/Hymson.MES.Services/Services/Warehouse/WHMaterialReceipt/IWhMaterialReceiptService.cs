using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WHMaterialReceipt;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;

namespace Hymson.MES.Services.Services.WHMaterialReceipt
{
    /// <summary>
    /// 服务接口（物料收货表）
    /// </summary>
    public interface IWhMaterialReceiptService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task CreateAsync(WhMaterialReceiptSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhMaterialReceiptSaveDto saveDto);

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
        Task<WhMaterialReceiptDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialReceiptDto>> GetPagedListAsync(WhMaterialReceiptPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询详情（物料收货表）
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        Task<IEnumerable<ReceiptMaterialDetailDto>> QueryDetailByReceiptIdAsync(long receiptId);


    }
}