using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（OQC检验单）
    /// </summary>
    public interface IQualOqcOrderService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualOqcOrderSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(QualOqcOrderSaveDto saveDto);

        /// <summary>
        /// 修改检验单状态
        /// </summary>
        /// <param name="updateStatusDto"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(UpdateStatusDto updateStatusDto);

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
        Task DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualOqcOrderDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualOqcOrderDto>> GetPagedListAsync(QualOqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取OQC检验单检验类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualOqcOrderTypeOutDto>> GetOqcOrderTypeAsync(long id);

        /// <summary>
        /// 校验样品条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<CheckBarCodeOutDto>> CheckBarCodeAsync(CheckBarCodeQuqryDto checkBarCodeQuqryDto);

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task SaveOrderAsync(QualOqcOrderExecSaveDto requestDto);

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task CompleteOrderAsync(QualOqcOrderCompleteDto requestDto);

        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IEnumerable<OQCAnnexOutDto>> SaveAttachmentAsync(QualOqcOrderSaveAttachmentDto requestDto);

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        Task DeleteAttachmentByIdAsync(long orderAnnexId);

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryDetailSamplePagedListAsync(OqcOrderParameterDetailPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 修改样品检验数据
        /// </summary>
        /// <param name="updateSampleDetailDto"></param>
        /// <returns></returns>
        Task UpdateSampleDetailAsync(UpdateSampleDetailDto updateSampleDetailDto);

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="oQCOrderUnqualifiedHandleDto"></param>
        /// <returns></returns>
        Task UnqualifiedHandleAnync(OQCOrderUnqualifiedHandleDto oQCOrderUnqualifiedHandleDto);

        /// <summary>
        /// 查询不合格样品数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryUnqualifiedPagedListAsync(OqcOrderParameterDetailPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取已检数据和样本数量
        /// </summary>
        /// <param name="sampleQtyAndCheckedQtyQueryDto"></param>
        /// <returns></returns>
        Task<SampleQtyAndCheckedQtyQueryOutDto> GetSampleQtyAndCheckedQtyAsync(SampleQtyAndCheckedQtyQueryDto sampleQtyAndCheckedQtyQueryDto);

        ///// <summary>
        ///// 根据ID查询类型
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //Task<IEnumerable<QualOqcOrderTypeDto>> QueryOrderTypeListByIdAsync(long orderId);
    }
}