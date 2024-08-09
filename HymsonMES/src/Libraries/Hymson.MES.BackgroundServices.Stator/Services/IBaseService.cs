﻿using Hymson.MES.Core.Domain.Process;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public interface IBaseService
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
        Task<StatorSummaryBo> ConvertDataTableAsync(DataTable dataTable, string producreCode, IEnumerable<string>? parameterCodes = null);

        /// <summary>
        /// 保存转换数据（附带参数）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="barCodes"></param>
        /// <param name="parameterCodes"></param>
        /// <returns></returns>
        Task<StatorSummaryBo> ConvertDataListAsync<T>(IEnumerable<T> entities, IEnumerable<string> barCodes, IEnumerable<string>? parameterCodes = null) where T : BaseOPEntity;

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
        /// <param name="buzKey"></param>
        /// <param name="waterLevel"></param>
        /// <param name="summaryBo"></param>
        /// <returns></returns>
        Task<int> SaveBaseDataWithCommitAsync(string buzKey, long waterLevel, StatorSummaryBo summaryBo);

    }
}
