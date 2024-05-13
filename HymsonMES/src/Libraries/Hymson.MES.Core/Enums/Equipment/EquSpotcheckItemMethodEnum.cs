using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 设备点检方式
    /// </summary>
    public enum EquSpotcheckItemMethodEnum : sbyte
    {
        /// <summary>
        /// 目视
        /// </summary>
        [Description("目视")]
        SightCheck = 1,

        /// <summary>
        /// 量测
        /// </summary>
        [Description("量测")]
        Measuration = 2,
        

    }
}
