using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public interface IManuEquipmentParameterService
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<EquipmentParameterDto> param);

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tabname"></param>
        /// <returns></returns>
        Task CreateEquipmentParameterTable(string tabname);
    }
}
