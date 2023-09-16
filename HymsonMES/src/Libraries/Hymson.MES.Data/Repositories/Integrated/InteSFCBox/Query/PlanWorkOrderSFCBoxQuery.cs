using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query
{
    public class PlanWorkOrderSFCBoxQuery
    {
        public long? Id { get; set; }
        public string? BoxCode { get; set; }
        public string? Grade { get; set; }

    }
}
