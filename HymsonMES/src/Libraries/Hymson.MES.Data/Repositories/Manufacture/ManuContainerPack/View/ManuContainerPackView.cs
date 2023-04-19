using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuContainerPackView : BaseEntity
    {
        public long SiteId { get; set; }
        public long ContainerBarCodeId { get; set; }
        public string BarCode { get; set; }
        public string LadeBarCode { get; set; }
        public long ProductId { get; set; }
    }
}
