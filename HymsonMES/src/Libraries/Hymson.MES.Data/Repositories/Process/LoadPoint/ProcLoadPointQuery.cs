/*
 *creator: Karl
 *
 *describe: 上料点表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点表 查询参数
    /// </summary>
    public class ProcLoadPointQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :上料点 
        /// </summary>
        public string? LoadPoint { get; set; }

    }

    #region 顷刻

    /// <summary>
    /// 上料点和设备编查询
    /// </summary>
    public class ProcLoadPointEquipmentQuery
    {
        /// <summary>
        /// 上料点或者设备编码列表
        /// </summary>
        public List<string> CodeList { get; set;} = new List<string>();
    }

    #endregion

}
