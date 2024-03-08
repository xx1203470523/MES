using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;

namespace Hymson.MES.Services.Services.EquEquipmentLoginRecord
{
    /// <summary>
    /// 服务接口（操作员登录记录）
    /// </summary>
    public interface IEquEquipmentLoginRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquEquipmentLoginRecordSaveDto saveDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquEquipmentLoginRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquEquipmentLoginRecordSaveDto saveDto);

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
        Task<EquEquipmentLoginRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentLoginRecordDto>> GetPagedListAsync(EquEquipmentLoginRecordPagedQueryDto pagedQueryDto);

    }
}