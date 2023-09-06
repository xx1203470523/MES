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
    public interface IManuEquipmentParameterRepository
    {

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<EquipmentParameterEntity> list, string tableName);


        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableSql"></param>
        /// <returns></returns>
        Task<int> CreateProductParameterTable(string tableSql);

        /// <summary>
        /// 获取创建表脚本
        /// </summary>
        /// <returns></returns>
        Task<string?> ShowCreateTable(string tableName);
    }
}
