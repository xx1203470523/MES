using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification
{
    /// <summary>
    /// 单体条码上料校验
    /// </summary>
    public class SingleBarCodeLoadingVerificationRequest : BaseRequest
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        ///操作类型 A-扫单体码 B-扫模组码 
        /// </summary>
        public string WorkType { get; set; } = string.Empty;

        /// <summary>
        /// 单体码
        /// </summary>
        public string Barcode { get; set; } = string.Empty;

    }
}
