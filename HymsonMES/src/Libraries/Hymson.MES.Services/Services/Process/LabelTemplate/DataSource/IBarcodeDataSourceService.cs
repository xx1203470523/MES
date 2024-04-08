using Hymson.MES.BackgroundServices.Dtos.Manufacture.LabelTemplate;
using Hymson.Print.Abstractions;

namespace Hymson.MES.BackgroundServices.Manufacture.PrintTemplate.ProductionBarcode
{
    /// <summary>
    /// 条码数据源
    /// </summary>
    public interface IBarcodeDataSourceService<T> where T : BasePrintData
    {
        /// <summary>
        ///获取模版数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<PrintStructDto<T>>> GetLabelTemplateDataAsync(BaseLabelTemplateDataDto param);
    }
}
