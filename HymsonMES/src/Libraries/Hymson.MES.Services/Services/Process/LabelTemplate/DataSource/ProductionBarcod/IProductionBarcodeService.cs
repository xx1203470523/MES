using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using Hymson.Print.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource.ProductionBarcod
{
    public  interface IProductionBarcodeService
    {
        Task<IEnumerable<PrintStructDto<ProductionBarcodeDto>>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param);
    }
}
