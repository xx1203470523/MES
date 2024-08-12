using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcMarking
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManuSfcMarkingCoreService
    {
        /// <summary>
        /// 组装Marking继承参数
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<(IEnumerable<ManuSfcMarkingEntity>, IEnumerable<ManuSfcMarkingExecuteEntity>)> GetMarkingInheritEntityAsync(ManuSfcMarkingBo bo);

        /// <summary>
        /// Marking拦截
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task MarkingInterceptAsync(MarkingInterceptBo bo);
    }
}
