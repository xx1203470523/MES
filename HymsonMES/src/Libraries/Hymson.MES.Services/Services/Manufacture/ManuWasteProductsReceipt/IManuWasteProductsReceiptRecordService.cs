using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（废成品入库记录）
    /// </summary>
    public interface IManuWasteProductsReceiptRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuWasteProductsReceiptRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuWasteProductsReceiptRecordSaveDto saveDto);

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
        Task<ManuWasteProductsReceiptRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuWasteProductsReceiptRecordDto>> GetPagedListAsync(ManuWasteProductsReceiptRecordPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询所有副产品入库信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ManuWasteProductsReceiptRecordDto>> QueryWasteProductsReceiptInfoListAsync();

    }
}