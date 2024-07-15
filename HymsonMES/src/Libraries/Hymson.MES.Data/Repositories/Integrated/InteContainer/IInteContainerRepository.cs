using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteContainer
{
    /// <summary>
    /// 仓储接口（容器维护）
    /// </summary>
    public interface IInteContainerRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteContainerEntity entity);

        /// <summary>
        /// 容器信息新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertInfoAsync(InteContainerInfoEntity entity);

        /// <summary>
        /// 容器货物新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertFreightAsync(IEnumerable<InteContainerFreightEntity> entity);

        /// <summary>
        /// 容器规格新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertSpecificationAsync(InteContainerSpecificationEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteContainerInfoEntity entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 删除（批量、容器货物）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command);

        /// <summary>
        /// 删除（容器参数）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteSpecificationByParentIdAsync(DeleteByParentIdCommand command);

        /// <summary>
        /// 根据ID获取Info数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteContainerInfoEntity> GetContainerInfoByIdAsync(long id);

        /// <summary>
        /// 根据容器ID获取容器规格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteContainerSpecificationEntity> GetSpecificationAsync(long id);

        /// <summary>
        /// 查询Freight列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteContainerFreightEntity>> GetFreightAsync(EntityByParentIdQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteContainerEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteContainerInfoEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteContainerView> GetInfoByIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteContainerInfoEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 通过关联ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteContainerEntity> GetByRelationIdAsync(InteContainerQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteContainerView>> GetPagedInfoAsync(InteContainerPagedQuery pagedQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);
    }
}
