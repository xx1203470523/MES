using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using Hymson.Print.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource
{
    /// <summary>
    /// 条码数据源
    /// </summary>
    public interface IBarcodeDataSourceService
    {
        /// <summary>
        ///获取模版数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> GetLabelTemplateData(BaseLabelTemplateDataDto param);
    }
}
