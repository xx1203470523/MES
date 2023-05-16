using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc
{
    /// <summary>
    /// 容器绑定条码数据
    /// </summary>
    public class QueryContainerBindSfcReaponse
    {
        /// <summary>
        /// 条码 
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 位置 
        /// </summary>
        public string Location { get; set; } = string.Empty;
    }
}
