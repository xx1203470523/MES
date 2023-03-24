/*
 *creator: Karl
 *
 *describe: 编码规则    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 编码规则 service接口
    /// </summary>
    public interface IInteCodeRulesService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteCodeRulesPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCodeRulesPageViewDto>> GetPageListAsync(InteCodeRulesPagedQueryDto inteCodeRulesPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        Task CreateInteCodeRulesAsync(InteCodeRulesCreateDto inteCodeRulesCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        Task ModifyInteCodeRulesAsync(InteCodeRulesModifyDto inteCodeRulesModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteCodeRulesAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesInteCodeRulesAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCodeRulesDetailViewDto> QueryInteCodeRulesByIdAsync(long id);
    }
}
