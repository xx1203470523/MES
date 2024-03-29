using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（FQC检验单）
    /// </summary>
    public interface IQualFqcOrderService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualFqcOrderSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(QualFqcOrderSaveDto saveDto);

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
        Task<QualFqcOrderDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualFqcOrderDto>> GetPagedListAsync(QualFqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> OperationOrderAsync(QualOrderOperationStatusDto requestDto);

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SaveOrderAsync(QualIqcOrderSaveDto requestDto);

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> CompleteOrderAsync(QualIqcOrderCompleteDto requestDto);

        /// <summary>
        /// 免检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //Task<int> FreeOrderAsync(QualIqcOrderFreeDto requestDto);

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> CloseOrderAsync(QualFqcOrderCloseDto requestDto);

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
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<long> GeneratedOrderAsync(GenerateInspectionDto requestDto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> UpdateOrderAsync(OrderParameterDetailSaveDto requestDto);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        //Task<PagedInfo<QualFqcOrderDto>> GetPagedListAsync(QualIqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryOrderTypeListByIdAsync(long orderId);

        /// <summary>
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId);

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSnapshotAsync(FQCParameterDetailQueryDto requestDto);

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSampleAsync(FQCParameterDetailQueryDto requestDto);

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<FQCParameterDetailDto>> QueryDetailSamplePagedListAsync(FQCParameterDetailPagedQueryDto pagedQueryDto);

    }
}