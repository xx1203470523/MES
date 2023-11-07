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


        // 2023.11.06 add
        /// <summary>
        /// 参数采集（面板用）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        //Task<int> ProductParameterCollectAsync(ProductProcessParameterBo bo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //Task<int> SaveAsync(IEnumerable<ParameterBo> param);

    }
}
