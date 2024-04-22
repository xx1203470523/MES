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
    public class ManuSfcService : IManuSfcService
    {
        /// <summary>
        /// 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcService(IManuSfcRepository manuSfcRepository) 
        {
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command)
        {
            return await _manuSfcRepository.UpdateQtyBySfcAsync(command);
        }
    }
}
