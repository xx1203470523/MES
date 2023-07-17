using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteJob.Query
{
    /// <summary>
    /// 
    /// </summary>
    public  class InteJobBusinessRelationByBusinessIdQuery
    {
        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum? LinkPoint { get; set; }

        /// <summary>
        /// 关联的业务表的ID
        /// </summary>
        public long BusinessId { get; set; }
    }
}
