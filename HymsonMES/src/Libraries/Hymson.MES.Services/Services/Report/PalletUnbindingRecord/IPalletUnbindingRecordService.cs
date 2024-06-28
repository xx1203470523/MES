using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.PalletUnbindingRecord
{
    public interface IPalletUnbindingRecordService
    {
        /// <summary>
        /// 根据查询报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<VehicleFreightRecordDto>> GetVehicleFreightRecorPageListAsync(VehicleFreightRecordQueryDto param);
    }
}