using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process.LabelTemplate.Utility
{
    /// <summary>
    /// 获取数据源
    /// </summary>
    public class LabelTemplateSourceDto
    {
        /// <summary>
        /// 数据源名
        /// </summary>
        public string DataSourceName { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public  IEnumerable<string>  BarCodes { get; set; }
    }
}
