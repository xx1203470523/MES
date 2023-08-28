using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（尾检检验单）
    /// </summary>
    public interface IQualIpqcInspectionTailService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualIpqcInspectionTailSaveDto saveDto);

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
        Task<QualIpqcInspectionTailDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取检验单已检样本列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualIpqcInspectionTailSampleDto>> GetPagedSampleListAsync(QualIpqcInspectionTailSamplePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据检验单ID获取检验单附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualIpqcInspectionTailAnnexDto>?> GetAttachmentListAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualIpqcInspectionTailDto>> GetPagedListAsync(QualIpqcInspectionTailPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(StatusChangeDto dto);

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        Task<int> InsertSampleDataAsync(List<QualIpqcInspectionTailSampleCreateDto> dataList);

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateSampleDataAsync(QualIpqcInspectionTailSampleUpdateDto param);

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> CompleteAsync(StatusChangeDto dto);

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> UnqualifiedHandleAsync(UnqualifiedHandleDto dto);

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> AttachmentAddAsync(AttachmentAddDto dto);

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> AttachmentDeleteAsync(long[] ids);

        /// <summary>
        /// 查询检验单样品应检参数并校验
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<SampleShouldInspectItemsDto>?> GetSampleShouldInspectItemsAsync(SampleShouldInspectItemsQueryDto query);
    }
}