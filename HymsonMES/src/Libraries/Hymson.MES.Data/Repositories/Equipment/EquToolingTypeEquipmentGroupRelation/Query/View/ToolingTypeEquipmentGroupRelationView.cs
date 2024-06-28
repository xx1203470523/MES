using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquToolingTypeEquipmentGroupRelation.Query.View
{
        public class ToolingTypeEquipmentGroupRelationView
    {
            /// <summary>
            /// 设备类型id
            /// </summary>
            public long ToolTypeId { get; set; }

            // <summary>
            /// 设备组Id
            /// </summary>
            public long EquipmentGroupId { get; set; }

        }
}
