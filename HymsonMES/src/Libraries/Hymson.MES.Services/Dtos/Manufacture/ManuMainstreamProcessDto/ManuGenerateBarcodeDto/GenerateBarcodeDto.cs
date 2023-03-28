using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto
{
    /// <summary>
    /// 生成条码实体
    /// </summary>
    public class GenerateBarcodeDto
    {
        /// <summary>
        /// 规则id
        /// </summary>
        public long CodeRuleId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 起始
        /// </summary>
        public bool IsTest { get; set; }=false;
    }
}
