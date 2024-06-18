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
            /// 主键id
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 备件类型id
            /// </summary>
            public long SparePartsGroupId { get; set; }

            // <summary>
            /// 设备组Id
            /// </summary>
            public long EquipmentGroupId { get; set; }

            /// <summary>
            /// 创建人
            /// </summary>
            public string CreatedBy { get; set; }

            /// <summary>
            /// 设备组编码
            /// </summary>
            public string EquipmentGroupCode { get; set; }

            /// <summary>
            /// 设备组名称
            /// </summary>
            public string EquipmentGroupName { get; set; }

            /// <summary>
            /// 创建时间
            /// </summary>
            public DateTime CreatedOn { get; set; }

            /// <summary>
            /// 最后修改人
            /// </summary>
            public string UpdatedBy { get; set; }

            /// <summary>
            /// 修改时间
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
}
