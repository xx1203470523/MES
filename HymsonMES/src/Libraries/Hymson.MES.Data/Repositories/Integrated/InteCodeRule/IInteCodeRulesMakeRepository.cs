/*
 *creator: Karl
 *
 *describe: 编码规则组成仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则组成仓储接口
    /// </summary>
    public interface IInteCodeRulesMakeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesMakeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteCodeRulesMakeEntity inteCodeRulesMakeEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCodeRulesMakeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCodeRulesMakeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCodeRulesMakeEntity inteCodeRulesMakeEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteCodeRulesMakeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<InteCodeRulesMakeEntity> inteCodeRulesMakeEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCodeRulesMakeEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCodeRulesMakeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteCodeRulesMakeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCodeRulesMakeEntity>> GetInteCodeRulesMakeEntitiesAsync(InteCodeRulesMakeQuery inteCodeRulesMakeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCodeRulesMakePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCodeRulesMakeEntity>> GetPagedInfoAsync(InteCodeRulesMakePagedQuery inteCodeRulesMakePagedQuery);
    }
}
