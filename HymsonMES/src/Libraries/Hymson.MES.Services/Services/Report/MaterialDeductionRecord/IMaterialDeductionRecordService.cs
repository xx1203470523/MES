using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.MaterialDeductionRecord
{
    public interface IMaterialDeductionRecordService
    {
        /// <summary>
        /// 扣料分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<MaterialDeductionRecordResultDto>> GetMaterialDeductionRecorPageListAsync(ComUsageReportPagedQueryDto param);
    }
}
