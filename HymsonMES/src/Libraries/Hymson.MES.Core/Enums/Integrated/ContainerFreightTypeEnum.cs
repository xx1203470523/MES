using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    public enum ContainerFreightTypeEnum : sbyte
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 1,
        /// <summary>
        /// 物料组
        /// </summary>
        [Description("物料组")]
        MaterialGroup = 2,
        /// <summary>
        /// 容器
        /// </summary>
        [Description("容器")]
        Container = 3,
    }
}
