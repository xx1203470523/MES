using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource
{
    public class BaseLabelTemplateDataDto
    {
        public long SiteId { get; set; }

        public IEnumerable<string> BarCodes { get; set; }
    }
}
