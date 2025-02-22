using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（事件类型维护）
    /// </summary>
    public interface IInteEventTypeService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<long> CreateAsync(InteEventTypeSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteEventTypeSaveDto saveDto);

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
        Task<InteEventTypeDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 查询事件类型
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryByWorkShopIdAsync(long workShopId);

        /// <summary>
        /// 根据ID获取关联事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventBaseDto>> QueryEventsByMainIdAsync(long id);

        /// <summary>
        /// 根据ID获取关联群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventTypeMessageGroupRelationDto>> QueryMessageGroupsByMainIdAsync(long id);

        /// <summary>
        /// 根据ID获取升级数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<InteEventTypeUpgradeDto>> GetUpgradeByMainIdAsync(InteEventTypeUpgradePagedQueryDto query);

        /// <summary>
        /// 根据ID获取推送规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventTypePushRuleDto>> QueryRulesByMainIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteEventTypeDto>> GetPagedListAsync(InteEventTypePagedQueryDto pagedQueryDto);

    }
}