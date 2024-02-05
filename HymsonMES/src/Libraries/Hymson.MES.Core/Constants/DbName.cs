using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Constants
{
    /// <summary>
    /// 数据库名称常量 注意：定义完常量需要在db_description表中配置相应库的数据库连接语句，此账号需要有创建或者删除表权限，以执行特殊的sql脚本
    /// </summary>
    public sealed  class DbName
    {
        /// <summary>
        /// mes主库
        /// </summary>
        public const string MES_MASTER = "mes_master";

        /// <summary>
        /// mes参数库
        /// </summary>
        public const string MES_MASTER_PARAMETER = "mes_master_parameter";
            
    }
}
