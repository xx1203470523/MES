using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox
{
    public interface IInteSFCBoxRepository
    {
        Task<int> InsertsAsync(IEnumerable<InteSFCBoxEntity> inteSFCBoxEntitys);
        /// <summary>
        /// 批量插入箱码关联表
        /// </summary>
        /// <param name="inteSFCBoxWorkOrderEntitys"></param>
        /// <returns></returns>
        Task<int> InsertSFCBoxWorkOrderAsync(IEnumerable<InteSFCBoxWorkOrderEntity> inteSFCBoxWorkOrderEntitys);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteSFCBoxEntity>> GetPagedInfoAsync(InteSFCBoxQueryRep pagedQuery);

        Task<PagedInfo<InteSFCBoxEntity>> GetBoxCodeAsync(InteSFCBoxQueryRep pagedQuery);

        /// <summary>
        /// 批量查询BoxCode 带OCVB差值计算
        /// </summary>
        /// <param name="boxCodes"></param>
        /// <returns></returns>
        Task<IEnumerable<InteSFCAllView>> GetByBoxCodesAsync(string[] batchNos);

        /// <summary>
        /// 获取批次码相关信息
        /// </summary>
        /// <param name="boxCodes"></param>
        /// <returns></returns>
        Task<IEnumerable<InteSFCBoxEntity>> GetManuSFCBoxAsync(InteSFCBoxEntityQuery query);

        /// <summary>
        /// 删除关联表数据，按工单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteSFCBoxWorkOrderAsync(long id);

        /// <summary>
        /// 按工单ID获取SFCbox信息
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderSFCBoxQuery>> GetByWorkOrderAsync(long workOrderId);

        /// <summary>
        /// 按ID获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteSFCBoxEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

    }
}
