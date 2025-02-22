/*
 *creator: Karl
 *
 *describe: job业务配置配置表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// job业务配置配置表仓储接口
    /// </summary>
    public interface IInteJobBusinessRelationRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobBusinessRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据JobIds批量获取数据
        /// </summary>
        /// <param name="jobIds"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetByJobIdsAsync(IEnumerable<long> jobIds);


        /// <summary>
        /// 更具业务id获取job
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetByJobByBusinessIdAsync(InteJobBusinessRelationByBusinessIdQuery query);
        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteJobBusinessRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobBusinessRelationEntity>> GetInteJobBusinessRelationEntitiesAsync(InteJobBusinessRelationQuery inteJobBusinessRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobBusinessRelationEntity>> GetPagedInfoAsync(InteJobBusinessRelationPagedQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByBusinessIdAsync(long id);

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByBusinessIdRangeAsync(IEnumerable<long> ids);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(long[] ids);
    }
}
