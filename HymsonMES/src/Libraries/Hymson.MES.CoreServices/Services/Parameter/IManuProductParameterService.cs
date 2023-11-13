using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Manufacture;
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
        /// 参数采集
        /// </summary>
        /// <returns></returns>
        Task CreateProductParameterTableAsync(string tabname);

        /// <summary>
        /// 根据工序创建数据库表
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        Task CreateProductParameterProcedureCodeTableAsync(long siteId, string procedureCode);

        /// <summary>
        /// 准备工序维度创建数据库表sql语句
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        string PrepareProductParameterProcedureCodeTableSql(long siteId, string procedureCode);


        // 2023.11.06 add
        /// <summary>
        /// 根据产品参数ID获取关联明细
        /// </summary>
        /// <param name="productParameterGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductParameterGroupDetailBo>> GetDetailsByProductParameterGroupIdAsync(long productParameterGroupId);

        /// <summary>
        /// 参数采集（面板用）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ProductParameterCollectAsync(ProductProcessParameterBo bo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="manufactureBo"></param>
        /// <param name="bos"></param>
        /// <returns></returns>
        Task<int> SaveAsync(ManufactureBo manufactureBo, IEnumerable<ParameterBo> bos);

    }
}
