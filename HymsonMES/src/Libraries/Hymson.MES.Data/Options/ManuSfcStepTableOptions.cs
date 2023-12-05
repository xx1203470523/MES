using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Options
{
    /// <summary>
    /// 步骤表相关配置
    /// </summary>
    public class ManuSfcStepTableOptions
    {
        /// <summary>
        /// 分表数量
        /// </summary>
        public uint Divides { get; set; } = 32;
    }
}
