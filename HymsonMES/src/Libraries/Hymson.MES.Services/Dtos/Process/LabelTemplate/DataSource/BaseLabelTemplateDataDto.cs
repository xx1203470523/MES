using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Dtos.Manufacture.LabelTemplate
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseLabelTemplateDataDto
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 模版
        /// </summary>
        public string? TemplateName { get; set; }

        /// <summary>
        /// 打印名称
        /// </summary>
        public string? PrintName { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public short PrintCount { get; set; }

        public IEnumerable<string> BarCodes { get; set; }
    }
}
