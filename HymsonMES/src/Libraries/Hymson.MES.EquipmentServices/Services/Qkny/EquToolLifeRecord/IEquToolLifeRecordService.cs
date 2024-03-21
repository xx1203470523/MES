using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;

namespace Hymson.MES.Services.Services.EquToolLifeRecord
{
    /// <summary>
    /// 服务接口（设备夹具寿命）
    /// </summary>
    public interface IEquToolLifeRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquToolLifeRecordSaveDto saveDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquToolLifeRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquToolLifeRecordSaveDto saveDto);

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
        Task<EquToolLifeRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolLifeRecordDto>> GetPagedListAsync(EquToolLifeRecordPagedQueryDto pagedQueryDto);

    }
}