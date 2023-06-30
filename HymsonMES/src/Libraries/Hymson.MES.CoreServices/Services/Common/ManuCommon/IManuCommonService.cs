using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Common.ManuCommon
{
    /// <summary>
    /// 生产公共类
    /// </summary>
    public interface IManuCommonService
    {
        /// <summary>
        /// 批量验证条码是否锁定
        /// </summary>
        /// <param name="procedureBo"></param>
        /// <returns></returns>
        Task VerifySfcsLockAsync(ManuProcedureBo procedureBo);

        /// <summary>
        /// 批量验证条码是否被容器包装
        /// </summary>
        /// <param name="sfcBos"></param>
        /// <returns></returns>
        Task VerifyContainerAsync(MultiSFCBo sfcBos);

        /// <summary>
        /// 验证条码BOM清单用量
        /// </summary>
        /// <param name="procedureBomBo"></param>
        /// <returns></returns>
        Task VerifyBomQtyAsync(ManuProcedureBomBo procedureBomBo);


        /// <summary>
        /// 获取工序关联的资源
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetProcResourceIdByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 判断上一工序是否随机工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<bool> IsRandomPreProcedureAsync(IsRandomPreProcedureBo randomPreProcedureBo);

        /// <summary>
        /// 获取生产工单
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="isVerifyActivation"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetProduceWorkOrderByIdAsync(GetProduceWorkOrderByIdBo produceWorkOrderByIdBo);

    }
}
