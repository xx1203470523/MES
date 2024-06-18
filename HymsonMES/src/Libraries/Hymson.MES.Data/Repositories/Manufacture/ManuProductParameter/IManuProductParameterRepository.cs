using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductParameter.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过程参数仓储接口
    /// </summary>
    public interface IManuProductParameterRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuProductParameterEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuProductParameterEntity> entities);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(EquipmentIdQuery query);


        /// <summary>
        ///获取条码最后采集参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<long?> GetLastProcedureIdAsync(LastManuProductParameterBySFCsQuery query);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuProductParameterEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuProductParameterEntity manuProductParameterEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateProductParameterByProcedureIdAsync(UpdateProductParameterByProcedureId command);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuProductParameterEntities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterEntity> manuProductParameterEntities);

        /// <summary>
        /// 查询ManuProductParameter
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetManuProductParameterAsync(ManuProductParameterQuery query);

        /// <summary>
        /// 参数分页查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductParameterView>> GetManuProductParameterPagedInfoAsync(ManuProductParameterPagedQuery queryParam);

        /// <summary>
        /// 产品过程参数报表
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductParameterEntity>> GetManuProductParameterReportPagedInfoAsync(ManuProductParameterReportPagedQuery queryParam);

        /// <summary>
        /// 求总数
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<int> GetManuProductParameterReportPagedCountAsync(ManuProductParameterReportPagedQuery pageQuery);
        /// <summary>
        /// 条码上报参数分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>  
        Task<PagedInfo<ManuProductParameterEntity>> GetManuProductParameterPageListAsync(ManuProductParameterTestPagedQuery queryParam);

        /// <summary>
        /// 获取参数条码
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>  
        Task<PagedInfo<string>> GetManuProductParameterSFCPageListAsync(ManuProductParameterTestPagedQuery queryParam);

        /// <summary>
        /// 根据SFC获取参数
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>  
        Task<IEnumerable<ManuProductParameterEntity>> GetManuProductParameterListBySFCAsync(ManuProductParameterTestPagedQuery queryParam);

        /// <summary>
        /// PACKEOL
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>  
        Task<PagedInfo<ManuProductParameterPackEOLView>> GetManuProductParameterPACKEOLPageListAsync(ManuProductParameterTestPagedQuery queryParam);

        /// <summary>
        /// CCS
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>  
        Task<PagedInfo<ManuProductParameterCCSView>> GetManuProductParameterCCSPageListAsync(ManuProductParameterTestPagedQuery queryParam);

        /// <summary>
        /// OCV
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>   
        Task<PagedInfo<ManuProductParameterOCVView>> GetManuProductParameterOCVPageListAsync(ManuProductParameterTestPagedQuery queryParam);

    }
}
