using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionBarcodeService : IBarcodeDataSourceService<BaseLabelTemplateDataDto, ProductionBarcodeDto>
    {
        /// <summary>
        /// 获取生产执行类 
        /// </summary>
        public ProductionBarcodeService()
        {

        } 

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ProductionBarcodeDto> GetLabelTemplateData(BaseLabelTemplateDataDto param)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
