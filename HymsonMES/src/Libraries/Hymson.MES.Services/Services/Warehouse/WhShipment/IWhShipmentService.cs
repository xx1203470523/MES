using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.WhShipment;

namespace Hymson.MES.Services.Services.WhShipment
{
    /// <summary>
    /// 服务接口（出货单）
    /// </summary>
    public interface IWhShipmentService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task CreateAsync(WhShipmentSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(WhShipmentSaveDto saveDto);

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
        Task<WhShipmentDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhShipmentDto>> GetPagedListAsync(WhShipmentPagedQueryDto pagedQueryDto);

        /// <summary>
        /// OQC检验单获取出货单
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhShipmentDto>> GetPagedListToOQCAsync(WhShipmentPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据条件查询出货单详情
        /// </summary>
        /// <param name="whShipmentQueryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<WhShipmentSupplierMaterialViewDto>> QueryShipmentSupplierMaterialAsync(WhShipmentQueryDto whShipmentQueryDto);
    }
}