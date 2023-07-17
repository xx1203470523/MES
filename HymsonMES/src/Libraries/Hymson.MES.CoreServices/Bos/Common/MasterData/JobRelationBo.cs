using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Common.MasterData
{
    /// <summary>
    /// 获取job关联点关联的作业
    /// </summary>
    public class JobRelationBo
    {

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { set; get; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { set; get; }

        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum? LinkPoint { get; set; }
    }
}
