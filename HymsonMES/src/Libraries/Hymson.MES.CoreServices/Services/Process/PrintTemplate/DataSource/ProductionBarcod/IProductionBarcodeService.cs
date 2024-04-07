using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.Print.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.ProductionBarcod
{
    public  interface IProductionBarcodeService
    {
        Task<IEnumerable<ProductionBarcodeDto>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param);
    }
}
