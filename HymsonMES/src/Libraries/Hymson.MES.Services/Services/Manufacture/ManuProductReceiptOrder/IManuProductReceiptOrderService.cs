using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（工单完工入库）
    /// </summary>
    public interface IManuProductReceiptOrderService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuProductReceiptOrderSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuProductReceiptOrderSaveDto saveDto);

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
        Task<ManuProductReceiptOrderDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductReceiptOrderDto>> GetPagedListAsync(ManuProductReceiptOrderPagedQueryDto pagedQueryDto);
        
        /// <summary>
        /// 根据工单查询入库记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductReceiptOrderDetailDto>> QueryByWorkIdAsync(long workOrderId);
    }
}