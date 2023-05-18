using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC
{
    /// <summary>
    /// 生成模组码-电芯堆叠
    /// </summary>
    public record GenerateModuleSFCModelDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }
    }
}
