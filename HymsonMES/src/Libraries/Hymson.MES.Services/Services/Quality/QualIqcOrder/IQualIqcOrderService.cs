using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（iqc检验单）
    /// </summary>
    public interface IQualIqcOrderService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualIqcOrderSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(QualIqcOrderSaveDto saveDto);

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
        Task<QualIqcOrderDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualIqcOrderDto>> GetPagedListAsync(QualIqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryOrderTypeListByIdAsync(long orderId);

        /// <summary>
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId);

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<InspectionParameterDetailDto>> QueryDetailSnapshotByIdAsync(long orderId);

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<InspectionParameterDetailDto>> QueryDetailSampleByIdAsync(long orderId);

    }
}