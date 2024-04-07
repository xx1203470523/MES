using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.Print.Abstractions;

namespace Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.BatchBarcode
{
    /// <summary>
    /// 原材料条码
    /// </summary>
    public interface IBatchBarcodeService
    {
        Task<IEnumerable<BatchBarcodeDto>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param);
    }
}
