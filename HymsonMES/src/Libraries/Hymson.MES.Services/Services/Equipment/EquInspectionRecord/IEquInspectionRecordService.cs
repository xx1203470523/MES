using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（点检记录表）
    /// </summary>
    public interface IEquInspectionRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquInspectionRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquInspectionRecordSaveDto saveDto);

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
        Task<EquInspectionRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquInspectionRecordDto>> GetPagedListAsync(EquInspectionRecordPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquInspectionOperateDto?> QueryByRecordIdAsync(long id);

        /// <summary>
        /// 开始校验
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> StartVerificationAsync(EquInspectionCompleteDto saveDto);

        /// <summary>
        /// 保存校验
        /// </summary>
        /// <returns></returns>
        Task<int> SaveVerificationnAsync(EquInspectionSaveDto saveDto);

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CompleteVerificationAsync(EquInspectionCompleteDto saveDto);

    }
}