using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.Resource
{
    public class ProcResourceConfigPrintView : ProcResourceConfigPrintEntity
    {
        /// <summary>
        /// PrintName
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印机IP
        /// </summary>
        public string PrintIp { get; set; }
    }
}
