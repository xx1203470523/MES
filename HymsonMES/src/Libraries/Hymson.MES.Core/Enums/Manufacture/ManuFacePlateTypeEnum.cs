using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 操作面板:类型
    /// </summary>
    public enum ManuFacePlateTypeEnum : short
    {
        /// <summary>
        /// 生产过站
        /// </summary>
        [Description("生产过站")]
        ProducePassingStation = 1,
        /// <summary>
        /// 在制品维修
        /// </summary>
        [Description("在制品维修")]
        ProductionRepair = 2
    }
}
