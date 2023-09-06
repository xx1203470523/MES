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
        ///查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterBySfcQuery param, string tableName);

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
        /// 获取创建表脚本
        /// </summary>
        /// <returns></returns>
        Task<string?> ShowCreateTableAsync(string tableName);
    }
}
