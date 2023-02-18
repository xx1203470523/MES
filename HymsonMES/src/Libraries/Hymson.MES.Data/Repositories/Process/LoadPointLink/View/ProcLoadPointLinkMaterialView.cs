using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcLoadPointLinkMaterialView : BaseEntity
    {
        /// <summary>
        /// 描述 :所属物料ID 
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 描述 :参考点 
        /// </summary>
        public string ReferencePoint { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }
    }
}
