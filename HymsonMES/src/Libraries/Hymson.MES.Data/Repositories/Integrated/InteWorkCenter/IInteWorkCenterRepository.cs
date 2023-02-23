using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.View;

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
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<InteWorkCenterEntity> GetByCodeAsync(EntityByCodeQuery param);

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
        Task<int> InsertRangAsync(List<InteWorkCenterEntity> param);

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
        Task<int> UpdateRangAsync(List<InteWorkCenterEntity> param);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeleteRangAsync(DeleteCommand param);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertInteWorkCenterRelationRangAsync(List<InteWorkCenterRelation> param);


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

        Task<int> InsertInteWorkCenterResourceRelationRangAsync(List<InteWorkCenterResourceRelation> param);

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
     
    }
}