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
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<ParameterDto> param);

        /// <summary>
        /// 更具工序参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>?> GetProductParameterListByProcedureAsync(QueryParameterByProcedureDto param);

        /// <summary>
        /// 更具条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterListBySFCAsync(QueryParameterBySfcDto param);

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateProductParameterByIdAsync(List<UpdateParameterDto> param);

        /// <summary>
        ///创建数据库表
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
        /// 参数采集（面板用）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> ProductParameterCollectAsync(ProductProcessParameterBo bo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> SaveAsync(IEnumerable<ParameterBo> param);

    }
}
