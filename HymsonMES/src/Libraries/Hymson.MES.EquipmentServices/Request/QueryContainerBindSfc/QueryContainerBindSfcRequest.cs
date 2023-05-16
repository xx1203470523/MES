using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc
{
    /// <summary>
    /// 容器绑定条码查询
    /// </summary>
    public class QueryContainerBindSfcRequest : BaseRequest
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContaineCode { get; set; } = string.Empty;
    }
}
