using Hymson.MES.Core.Domain.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.QualFqcOrderSample.View
{
    public class SampleFqcOrderView: QualFqcOrderEntity
    {
        public string Barcode { get; set; }
    }
}
