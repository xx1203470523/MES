using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Options
{
    /// <summary>
    /// 数据库连接字符串
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        /// WMS连接字符串
        /// </summary>
        public string WMSConnectionString { get; set; } = "";
    }
}
