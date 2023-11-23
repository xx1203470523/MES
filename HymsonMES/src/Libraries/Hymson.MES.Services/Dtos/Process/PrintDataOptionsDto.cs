using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 打印类选项
    /// </summary>
    public class PrintClassOptionDto
    {
        public string Label { get; set; }
        public object Value { get; set; }

        public IEnumerable<PrintClassPropertyOptionDto> PrintClassPropertyOptions { get; set; }
    }

    /// <summary>
    /// 打印类下的属性选项
    /// </summary>
    public class PrintClassPropertyOptionDto 
    {
        public string Label { get; set; }
        public object Value { get; set; }
    }
}
