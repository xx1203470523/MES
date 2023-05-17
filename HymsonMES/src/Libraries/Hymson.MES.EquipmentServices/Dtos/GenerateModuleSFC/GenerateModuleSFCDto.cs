using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC
{
    /// <summary>
    /// 请求生成模组码-电芯堆叠
    /// </summary>
    public record GenerateModuleSFCDto : BaseDto
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 生产个数
        /// </summary>
        public int Qty { get; set; } = 0;
    }
}
