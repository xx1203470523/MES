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
        Task<int> InsertRangeAsync(IEnumerable<EquipmentParameterEntity> list);

        /// <summary>
        /// 获取建表SQL语句
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        string PrepareEquipmentParameterBySequenceTableSql(int sequence);
    }
}
