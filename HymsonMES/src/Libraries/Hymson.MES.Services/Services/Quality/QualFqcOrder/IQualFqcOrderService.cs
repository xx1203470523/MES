using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using static Hymson.MES.Services.Dtos.Quality.QualFqcParameterGroup;

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
        Task<int> CreateAsync(QualFqcOrderCreateDto saveDto);

        /// <summary>
        /// 创建(测试条码产出时自动生成功能)
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(QualFqcOrderCreateTestDto saveDto);

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
        Task<int> OperationOrderAsync(OrderOperationStatusDto requestDto);

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SaveOrderAsync(QualFqcOrderSampleSaveDto requestDto);

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> CompleteOrderAsync(QualFqcOrderCompleteDto requestDto);

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
        Task<int> SaveAttachmentAsync(QualFqcOrderSaveAttachmentDto requestDto);

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
        //Task<int> DeleteOrdersAsync(long[] ids);

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
        Task<int> UpdateOrderAsync(FQCParameterDetailSaveDto requestDto);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        //Task<PagedInfo<QualFqcOrderDto>> GetPagedListAsync(QualIqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 查询检验项目组,样本数量
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<QualFqcParameterGroupSnapshootOut> QuerySnapshootByIdAsync(long orderId);

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
        /// 获取参数项目
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcParameterGroupEntity>> VerificationParametergroupAsync(ParameterGroupQuery requestDto);

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<FQCParameterDetailDto>> QueryDetailSamplePagedListAsync(FQCParameterDetailPagedQueryDto pagedQueryDto);

    }
}