using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPoint.View;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
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

        /// <summary>
        /// 获取上料点或者设备
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<PointOrEquipmentView>> GetPointOrEquipmmentAsync(ProcLoadPointEquipmentQuery query)
        {
            if(query == null || query.CodeList == null || query.CodeList.Count != 2)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45074));
            }
            var dbList = await _procLoadPointRepository.GetPointOrEquipmmentAsync(query);
            //if (dbList.IsNullOrEmpty() == true || dbList.Count() != query.CodeList.Count)
            //{
            //    //上料点会关联多个资源
            //    throw new CustomerValidationException(nameof(ErrorCode.MES45074));
            //}
            var dbCode = dbList.Where(m => m.Code == query.CodeList[0]).FirstOrDefault();
            if(dbCode == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45074));
            }
            dbCode = dbList.Where(m => m.Code == query.CodeList[1]).FirstOrDefault();
            if(dbCode == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45074));
            }
            return dbList.ToList();
        }

        /// <summary>
        /// 获取上料点
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointEntity> GetProcLoadPointAsync(ProcLoadPointQuery query)
        {
            var dbModel = await _procLoadPointRepository.GetProcLoadPointAsync(query);
            if (dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45070));
            }
            return dbModel;
        }

    }
}
