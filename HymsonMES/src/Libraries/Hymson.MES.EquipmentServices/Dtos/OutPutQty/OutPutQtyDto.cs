using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.OutPutQty
{
    /// <summary>
    /// 产出上报数量
    /// </summary>
    public record OutPutQtyDto : BaseDto
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
        public IEnumerable<NGList>? NGList { get; set; }

        /// <summary>
        /// 产品参数列表 可空
        /// </summary>
        public IEnumerable<ParamList>? ParamList { get; set; }

        /// <summary>
        /// 绑定原材料 可空
        /// </summary>
        public string[]? BindFeedingCodes { get; set; }

        /// <summary>
        /// 绑定投入产品，半成品 可空
        /// </summary>
        public string[]? BindProductCodes { get; set; }

    }

    /// <summary>
    /// 产出NG列表
    /// </summary>
    public class NGList
    {
        /// <summary>
        /// NG代码 
        /// </summary>
        public string NGCode { get; set; } = string.Empty;

        /// <summary>
        /// NG数量 
        /// </summary>
        public int NGQty { get; set; }
    }

    /// <summary>
    /// 产出NG列表
    /// </summary>
    public class ParamList
    {
        /// <summary>
        /// 参数代码 
        /// </summary>
        public string ParamCode { get; set; } = string.Empty;

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = string.Empty;
        /// <summary>
        /// 时间戳 
        /// </summary>
        public string Timestamp { get; set; } = string.Empty;
    }
}
