using Hymson.MES.CoreServices.Dtos.Qkny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 物料加载
    /// </summary>
    public interface IManuFeedingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto);
    }
}
