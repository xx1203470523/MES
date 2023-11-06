using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Parameter.ManuProductParameter
{
    /// <summary>
    /// 产品过程参数
    /// </summary>
    public interface IManuProductParameterRepository
    {

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
        ///查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterBySfcQuery param, string tableName);

        /// <summary>
        /// 更具条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterBySfcQuery param);

        /// <summary>
        /// 更具工序获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterByProcedureIdQuery param);

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
        /// 创建数据库表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> CreateProductParameterTableAsync(string tableName);

        /// <summary>
        /// 准备工序维度创建数据库表sql语句
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        string PrepareProductParameterProcedureCodeTableSql(long siteId, long procedureId);

        /// <summary>
        /// 获取创建表脚本
        /// </summary>
        /// <returns></returns>
        Task<string?> ShowCreateTableAsync(string tableName);
    }
}
