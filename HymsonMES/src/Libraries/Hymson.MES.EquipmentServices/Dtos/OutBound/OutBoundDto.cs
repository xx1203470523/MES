using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.OutBound
{
    /// <summary>
    /// 出站
    /// </summary>
    public record OutBoundDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 是否合格
        /// 0不合格,1合格
        /// </summary>
        public int? Passed { get; set; }

        /// <summary>
        /// 出站参数
        /// </summary>
        public OutBoundParam[]? ParamList { get; set; }

        /// <summary>
        /// 绑定的物料批次条码列表
        /// </summary>
        public string[]? BindFeedingCodes { get; set; }

        /// <summary>
        /// Ng代码
        /// </summary>
        public OutBoundNG[]? NG { get; set; }
    }

}
