using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcResourceView:ProcResourceEntity
    {
        /// <summary>
        /// 资源
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResTypeName { get; set; }
    }
}
