using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind
{
    public class ManuMergeDto
    {
        public IEnumerable<string> Barcodes { get; set; }
        public long SiteId { get; set; }
        public long ProcedureId { get; set; }
        public string TargetSFC { get; set; }
    }
}
