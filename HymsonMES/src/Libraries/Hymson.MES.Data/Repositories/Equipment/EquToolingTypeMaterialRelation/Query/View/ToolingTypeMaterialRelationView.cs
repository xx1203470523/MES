using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquToolingTypeMaterialRelation.Query.View
{
        public class ToolingTypeMaterialRelationView
    {

            /// <summary>
            /// 工具类型id
            /// </summary>
            public long ToolTypeId { get; set; }

            // <summary>
            /// 物料Id
            /// </summary>
            public long MaterialId { get; set; }
        }
}
