/*
 *creator: Karl
 *
 *describe: 降级规则    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级规则 service接口
    /// </summary>
    public interface IManuDowngradingRuleService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuDowngradingRulePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingRuleDto>> GetPagedListAsync(ManuDowngradingRulePagedQueryDto manuDowngradingRulePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingRuleCreateDto"></param>
        /// <returns></returns>
        Task CreateManuDowngradingRuleAsync(ManuDowngradingRuleCreateDto manuDowngradingRuleCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuDowngradingRuleModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuDowngradingRuleAsync(ManuDowngradingRuleModifyDto manuDowngradingRuleModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuDowngradingRuleAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuDowngradingRuleAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuDowngradingRuleDto> QueryManuDowngradingRuleByIdAsync(long id);

        /// <summary>
        /// 获取所有降级规则数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingRuleEntity>> GetAllManuDowngradingRuleEntitiesAsync();

        /// <summary>
        /// 修改降级规则的排序号
        /// </summary>
        /// <param name="manuDowngradingRuleChangeSerialNumberDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task UpdateSerialNumbersAsync(List<ManuDowngradingRuleChangeSerialNumberDto> manuDowngradingRuleChangeSerialNumberDtos);
    }
}
