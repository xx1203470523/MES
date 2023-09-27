using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary
{
    public interface IManuSfcSummaryService
    {

        /// <summary>
        /// 执行生产统计
        /// </summary>
        /// <returns></returns>
        Task ExecutStatisticAsync(string userId, long siteId);
    }
}
