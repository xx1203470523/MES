using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 条码表
    /// </summary>
    public interface IManuSfcService
    {
        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command);
    }
}
