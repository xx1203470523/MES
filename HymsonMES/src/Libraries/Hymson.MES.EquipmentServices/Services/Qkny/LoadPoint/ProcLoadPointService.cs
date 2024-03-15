using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint
{
    /// <summary>
    /// 上料
    /// </summary>
    public class ProcLoadPointService : IProcLoadPointService
    {
        /// <summary>
        /// 上料点仓储
        /// </summary>
        private readonly IProcLoadPointRepository _procLoadPointRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcLoadPointService(IProcLoadPointRepository procLoadPointRepository)
        {
            _procLoadPointRepository = procLoadPointRepository;
        }

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointEntity> GetProcLoadPointEntitiesAsync(ProcLoadPointQuery procLoadPointQuery)
        {
            var dbList = await _procLoadPointRepository.GetProcLoadPointEntitiesAsync(procLoadPointQuery);
            if(dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45070));
            }
            return dbList.First();
        }
    }
}
