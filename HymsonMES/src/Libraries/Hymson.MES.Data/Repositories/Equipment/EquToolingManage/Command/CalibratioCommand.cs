using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquToolingManage.Command
{
    /// <summary>
    /// 校准
    /// </summary>
    public class CalibratioCommandCommand: UpdateCommand
    {
        /// <summary>
        /// 主键
        /// </summary>
        public  long Id { get; set; }

        /// <summary>
        /// 最后检验时间
        /// </summary>
        public  DateTime LastVerificationTime { get; set; }
    }
}
