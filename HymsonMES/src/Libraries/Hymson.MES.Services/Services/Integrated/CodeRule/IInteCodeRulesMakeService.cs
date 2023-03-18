/*
 *creator: Karl
 *
 *describe: 编码规则组成    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
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
    /// 编码规则组成 service接口
    /// </summary>
    public interface IInteCodeRulesMakeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="inteCodeRulesMakePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCodeRulesMakeDto>> GetPageListAsync(InteCodeRulesMakePagedQueryDto inteCodeRulesMakePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesMakeDto"></param>
        /// <returns></returns>
        Task CreateInteCodeRulesMakeAsync(InteCodeRulesMakeCreateDto inteCodeRulesMakeCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesMakeDto"></param>
        /// <returns></returns>
        Task ModifyInteCodeRulesMakeAsync(InteCodeRulesMakeModifyDto inteCodeRulesMakeModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteInteCodeRulesMakeAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesInteCodeRulesMakeAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCodeRulesMakeDto> QueryInteCodeRulesMakeByIdAsync(long id);
    }
}
