using Hymson.Authentication.JwtBearer;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.Snowflake;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.EquEquipment
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public class EquEquipmentService : IEquEquipmentService
    {
        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquEquipmentService(IEquEquipmentRepository equEquipmentRepository)
        {
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResAllAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResAllAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        /// 获取设备资源对应的线体基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResLineAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResLineAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        ///  获取设备资源对应的工序基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResProcedureAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResProcedureAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// 用于化成NG电芯上报，此时实际发生不良的工序是可能是上面的多个工序出现的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<EquEquipmentResAllView>> GetMultEquResAllAsync(MultEquResAllQuery query)
        {
            var list = await _equEquipmentRepository.GetMultEquResAllAsync(query);
            if (list == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return list.ToList(); ;
        }

        /// <summary>
        /// 获取设备token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> GetEquTokenAsync(QknyBaseDto dto)
        {
            EquResAllQuery query = new EquResAllQuery() { EquipmentCode = dto.EquipmentCode };
            return await _equEquipmentRepository.GetEquTokenSqlAsync(query);
        }

    }
}
