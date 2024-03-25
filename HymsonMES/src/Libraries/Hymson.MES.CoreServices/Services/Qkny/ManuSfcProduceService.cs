using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Qkny
{
    /// <summary>
    /// 在制品服务
    /// </summary>
    public class ManuSfcProduceService : IManuSfcProduceService
    {
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcProduceService(IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }

        /// <summary>
        /// 根据条码更改条码状态
        /// 用于设备接口产出米数上报
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command)
        {
            return await _manuSfcProduceRepository.UpdateQtyBySfcAsync(command);
        }

        /// <summary>
        /// 获取设备最近条码
        /// </summary>
        /// <returns></returns>
        public async Task<ManuSfcProductMaterialView> GetEquipmentNewestSfc(ManuSfcEquipmentNewestQuery query)
        {
            var dbModel = await _manuSfcProduceRepository.GetEquipmentNewestSfc(query);
            if (dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45090));
            }
            return dbModel!;
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcQuery"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetBySFCAsync(ManuSfcProduceBySfcQuery query)
        {
            var dbModel = await _manuSfcProduceRepository.GetBySFCAsync(query);
            if (dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45090));
            }
            return dbModel!;
        }
    }
}
