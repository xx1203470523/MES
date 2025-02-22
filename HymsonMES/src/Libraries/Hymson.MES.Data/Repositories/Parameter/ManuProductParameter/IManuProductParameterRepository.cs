﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Repositories.Common.Query;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Parameter
{
    /// <summary>
    /// 产品过程参数
    /// </summary>
    public interface IManuProductParameterRepository
    {
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
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuProductParameterEntity> list, string tableName);

        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuProductParameterEntity> list);

        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> InsertRangeMavelAsync(IEnumerable<ManuProductParameterEntity> list);

        /// <summary>
        /// 获取马威参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetManuParamMavelAsync(EntityByWaterMarkQuery query);

        /// <summary>
        /// 获取马威NG参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetManuNgParamMavelAsync(EntityByWaterMarkQuery query);

        /// <summary>
        ///查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntitiesAsync(ManuProductParameterBySfcQuery param, string tableName);

        /// <summary>
        /// 更具条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterBySFCEntitiesAsync(ManuProductParameterBySfcQuery param);

        /// <summary>
        /// 更具工序获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterByProcedureIdEntitiesAsync(ManuProductParameterByProcedureIdQuery param);

        /// <summary>
        /// 修改参数值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterUpdateCommand> list);

        /// <summary>
        /// 修改参数值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterUpdateCommand> list, string tableName);

        /// <summary>
        /// 准备工序维度创建数据库表sql语句
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        string PrepareProductParameterProcedureIdTableSql(long siteId, long procedureId);

        /// <summary>
        /// 根据工序或者条码分页查询产品参数
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductParameterEntity>> GetParametesEntitiesAsync(ManuProductParameterPagedQuery pagedQuery);

         Task<int> PrepareProductParameterSFCTable(int index);
         Task<int> PrepareProductParameterProcedureldTable(long siteId, long procedureId);
    }
}
