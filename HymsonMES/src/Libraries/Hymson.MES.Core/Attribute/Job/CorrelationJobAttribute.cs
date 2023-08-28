using Hymson.MES.Core.Enums.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Attribute.Job
{
    [AttributeUsage(AttributeTargets.All)]
    public class CorrelationJobAttribute : System.Attribute
    {
        public CorrelationJobAttribute(ConnectionTypeEnum connectionType)
        {
            this.ConnectionType = connectionType;
        }

        /// <summary>
        /// 关联类型
        /// </summary>
        public ConnectionTypeEnum ConnectionType { get; set; } = ConnectionTypeEnum.procedureAndResource;
    }
}
