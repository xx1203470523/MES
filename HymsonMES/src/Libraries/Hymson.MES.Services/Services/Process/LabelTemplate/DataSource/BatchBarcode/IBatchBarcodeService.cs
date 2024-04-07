using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using Hymson.Print.Abstractions;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource.BatchBarcode
{
    /// <summary>
    /// 原材料条码
    /// </summary>
    public interface IBatchBarcodeService
    {
        Task<IEnumerable<PrintStructDto<BatchBarcodeDto>>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param);
    }
}
