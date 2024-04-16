using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture.ManuOutbound
{
    public interface IManuOutboundService
    {
        /// <summary>
        ///产出确认
        /// </summary>
        /// <param name="outputConfirmDto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseDto>> OutputConfirmAsync(OutputConfirmDto outputConfirmDto);
    }
}
