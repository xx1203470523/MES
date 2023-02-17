/*
 *creator: Karl
 *
 *describe: QualUnqualifiedGroupEntity仓储类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-02-13 02:05:50
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// QualUnqualifiedGroupEntity仓储接口
    /// </summary>
    public interface IQualUnqualifiedGroupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualUnqualifiedGroupEntity qualUnqualifiedGroupEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualUnqualifiedGroupEntity> qualUnqualifiedGroupEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualUnqualifiedGroupEntity qualUnqualifiedGroupEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualUnqualifiedGroupEntity> qualUnqualifiedGroupEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedGroupEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualUnqualifiedGroupQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupEntity>> GetQualUnqualifiedGroupEntitiesAsync(QualUnqualifiedGroupQuery qualUnqualifiedGroupQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualUnqualifiedGroupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedGroupEntity>> GetPagedInfoAsync(QualUnqualifiedGroupPagedQuery qualUnqualifiedGroupPagedQuery);

        /// <summary>
        /// 插入不合格代码组关联不合格代码
        /// </summary>
        /// <param name="qualUnqualifiedCodeGroupRelationList"></param>
        /// <returns></returns>
        Task<int> AddQualUnqualifiedCodeGroupRelationAsync(List<QualUnqualifiedCodeGroupRelation> qualUnqualifiedCodeGroupRelationList);

        /// <summary>
        /// 删除不合格代码组关联不合格代码
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long groupId);

        /// <summary>
        /// 插入不合格代码组关联工序
        /// </summary>
        /// <param name="qualUnqualifiedGroupProcedureRelationList"></param>
        /// <returns></returns>
        Task<int> AddQualUnqualifiedGroupProcedureRelationAsync(List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList);

        /// <summary>
        /// 删除不合格代码组关联工序
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long groupId);
    }
}
