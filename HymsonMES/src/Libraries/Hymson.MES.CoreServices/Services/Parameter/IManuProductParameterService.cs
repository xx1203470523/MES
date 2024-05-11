using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public interface IManuProductParameterService
    {
        /// <summary>
        /// 更具工序参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>?> GetProductParameterListByProcedureAsync(QueryParameterByProcedureDto param);

        /// <summary>
        /// 参数采集（产品过程参数）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ProductProcessCollectAsync(ProductProcessParameterBo bo);

        /// <summary>
        /// 参数采集（产品过程参数）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ProductProcessCollectAsync(ProductParameterCollectBo bo);
    }
}
