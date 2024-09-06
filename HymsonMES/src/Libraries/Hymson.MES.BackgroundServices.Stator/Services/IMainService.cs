using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public interface IMainService
    {
        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <param name="producreCode"></param>
        /// <returns></returns>
        Task<BaseStatorBo> GetStatorBaseConfigAsync(string producreCode = "");

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="producreCode"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataTableWireAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null);

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="producreCode"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataTableInnerAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null);

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="producreCode"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataTableOuterAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null);

        /// <summary>
        /// 获取参数编码
        /// </summary>
        /// <param name="parameterCodes"></param>
        /// <param name="statorBo"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcParameterEntity>> GetParameterEntitiesAsync(IEnumerable<string> parameterCodes, BaseStatorBo statorBo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataAsync(StatorSummaryBo summaryBo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataWithCommitAsync(StatorSummaryBo summaryBo);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="buzKey"></param>
        /// <param name="waterLevel"></param>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataWithCommitAsync(string buzKey, long waterLevel, StatorSummaryBo summaryBo);



        #region 仓储方法
        /// <summary>
        /// 批量获取条码（MES）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetSFCEntitiesAsync(long siteId, IEnumerable<string> barCodes);

        /// <summary>
        /// 批量获取（条码信息）
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetSFCInfoEntitiesAsync(IEnumerable<long> sfcIds);

        /// <summary>
        /// 批量获取（物料信息）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcMaterialEntity> GetMaterialEntityAsync(EntityByCodeQuery query);

        /// <summary>
        /// 批量获取（物料信息）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetMaterialEntitiesAsync(IEnumerable<long> ids);

        /// <summary>
        /// 批量获取（库存条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="barCodes"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetMaterialInventoryEntitiesAsync(long siteId, IEnumerable<string> barCodes);

        /// <summary>
        /// 批量获取（铜线条码）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WireBarCodeEntity>> GetWireBarCodeEntitiesAsync(long siteId, IEnumerable<string> ids);

        /// <summary>
        /// 批量读取条码（定子）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<StatorBarCodeEntity>> GetStatorBarCodeEntitiesAsync(StatorBarCodeQuery query);

        /// <summary>
        /// 批量获取（定子条码）
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        Task<IEnumerable<long>> GetInnerIdsByNullColumnAsync(string columnName);

        /// <summary>
        /// 批量获取（定子铜线关系）
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<StatorWireRelationEntity>> GetStatorWireRelationEntitiesAsync(long siteId, IEnumerable<string> ids);

        #endregion


    }
}
