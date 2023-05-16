using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Request.OutPutQty
{
    /// <summary>
    /// 产出上报数量
    /// </summary>
    public class OutPutQtyRequest : BaseRequest
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        ///合格数量
        /// </summary>
        public decimal OKQty { get; set; }

        /// <summary>
        /// 产出NG列表 可空
        /// </summary>
        public object[]? NGList { get; set; }

        /// <summary>
        /// 产品参数列表 可空
        /// </summary>
        public object[]? ParamList { get; set; }

        /// <summary>
        /// 绑定原材料 可空
        /// </summary>
        public string[]? BindFeedingCodes { get; set; }

        /// <summary>
        /// 绑定投入产品，半成品 可空
        /// </summary>
        public string[]? BindProductCodes { get; set; }

    }
}
