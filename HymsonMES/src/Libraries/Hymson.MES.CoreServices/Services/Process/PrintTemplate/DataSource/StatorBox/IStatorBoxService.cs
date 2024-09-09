using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.StatorBox
{
    /// <summary>
    /// 定子装箱
    /// </summary>
    public interface IStatorBoxService
    {
        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<StatorBoxDto>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param);
    }
}
