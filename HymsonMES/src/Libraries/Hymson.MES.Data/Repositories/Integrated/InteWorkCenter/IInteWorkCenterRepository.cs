using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.View;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository
{
    /// <summary>
    /// 工作中心表仓储
    /// @author admin
    /// @date 2023-02-22
    public interface IInteWorkCenterRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<InteWorkCenterEntity>> GetPagedInfoAsync(InteWorkCenterPagedQuery param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据类型获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetByTypeAndParentIdAsync(InteWorkCenterByTypeQuery query);

        /// <summary>
        /// 获取当前站点下面的所有工厂/车间/产线
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetWorkCenterListByTypeAsync(EntityByTypeQuery query);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetByCodeAsync(EntityByCodeQuery param);

        /// <summary>
        /// 根据编码获取数据（慎用）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetByAllSiteCodeAsync(EntityByCodeQuery param);

        /// <summary>
        /// 根据资源ID获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetByResourceIdAsync(long resourceId);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteWorkCenterEntity param);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertRangAsync(IEnumerable<InteWorkCenterEntity> param);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteWorkCenterEntity param);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateRangAsync(IEnumerable<InteWorkCenterEntity> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeleteRangAsync(DeleteCommand param);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteWorkCenterRelationByParentIdsAsync(DeleteCommand command);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteResourceRelationByParentIdsAsync(DeleteCommand command);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertInteWorkCenterRelationRangAsync(IEnumerable<InteWorkCenterRelation> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> RealDelteInteWorkCenterRelationRangAsync(long id);

        /// <summary>
        /// 获取工作中心关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterRelationView>> GetInteWorkCenterRelationAsync(long id);

        /// <summary>
        /// 获取工作中心关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterRelation>> GetInteWorkCenterRelationEntityAsync(InteWorkCenterRelationQuery inteWorkCenterRelationQuery);

        /// <summary>
        /// 根据资源ID获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetWorkCenterIdByResourceIdAsync(IEnumerable<long> resourceIds);

        /// <summary>
        /// 查询产线下面的资源ID集合
        /// </summary>
        /// <param name="workCenterIds"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetResourceIdsByWorkCenterIdAsync(long[] workCenterIds);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertInteWorkCenterResourceRelationRangAsync(IEnumerable<InteWorkCenterResourceRelation> param);

        /// <summary>
        /// 根据下级工作中心Id获取上级工作中心
        /// (只获取一级)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetHigherInteWorkCenterAsync(long id);

        /// <summary>
        /// 根据下级工作中心Id获取上级工作中心
        /// (只获取一级)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetHigherInteWorkCenterAsync(IEnumerable<long> id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> RealDelteInteWorkCenterResourceRelationRangAsync(long id);

        /// <summary>
        /// 获取工作中心关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterResourceRelationView>> GetInteWorkCenterResourceRelatioAsync(long id);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="workCenterQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetEntitiesAsync(InteWorkCenterQuery workCenterQuery);

        /// <summary>
        /// 根据条件查询（慎用）
        /// </summary>
        /// <param name="workCenterQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteWorkCenterEntity>> GetAllSiteEntitiesAsync(InteWorkCenterQuery workCenterQuery);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="workCenterQuery"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetEntitieAsync(InteWorkCenterFirstQuery workCenterQuery);
        Task<IEnumerable<InteWorkCenterResourceRelationView>> GetWorkCenterResourceRelationAsync(IEnumerable<long> resourceIds, long id);
    }
}