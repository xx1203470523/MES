using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon
{
    public class BarcodeDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { set; get; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }
    }
}
