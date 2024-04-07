using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;

namespace Hymson.MES.Services.Services.ManuFillingDataRecord
{
    /// <summary>
    /// 服务接口（补液数据上传记录）
    /// </summary>
    public interface IManuFillingDataRecordService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(ManuFillingDataRecordSaveDto saveDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuFillingDataRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuFillingDataRecordSaveDto saveDto);

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
        Task<ManuFillingDataRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFillingDataRecordDto>> GetPagedListAsync(ManuFillingDataRecordPagedQueryDto pagedQueryDto);

    }
}