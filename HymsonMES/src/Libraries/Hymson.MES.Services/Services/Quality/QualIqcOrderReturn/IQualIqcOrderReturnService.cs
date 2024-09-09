using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（iqc检验单）
    /// </summary>
    public interface IQualIqcOrderReturnService
    {
        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<long> GeneratedOrderAsync(GenerateOrderReturnDto requestDto);

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> OperationOrderAsync(QualOrderReturnOperationStatusDto requestDto);

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SaveOrderAsync(QualIqcOrderReturnSaveDto requestDto);


        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SaveAttachmentAsync(QualIqcOrderSaveAttachmentDto requestDto);

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        Task<int> DeleteAttachmentByIdAsync(long orderAnnexId);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteOrdersAsync(long[] ids);


        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualIqcOrderReturnBaseDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 取消检验单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> CancelOrderAsync(long id);

        /// <summary>
        /// 查询检验单明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualIqcOrderReturnDetailDto>?> QueryOrderDetailAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualIqcOrderReturnDto>> GetPagedListAsync(QualIqcOrderReturnPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId);

    }
}