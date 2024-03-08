using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;

namespace Hymson.MES.Services.Services.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// 服务接口（CCD文件上传完成）
    /// </summary>
    public interface ICcdFileUploadCompleteRecordService
    {
        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<CcdFileUploadCompleteRecordSaveDto> saveDtoList);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(CcdFileUploadCompleteRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(CcdFileUploadCompleteRecordSaveDto saveDto);

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
        Task<CcdFileUploadCompleteRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<CcdFileUploadCompleteRecordDto>> GetPagedListAsync(CcdFileUploadCompleteRecordPagedQueryDto pagedQueryDto);

    }
}