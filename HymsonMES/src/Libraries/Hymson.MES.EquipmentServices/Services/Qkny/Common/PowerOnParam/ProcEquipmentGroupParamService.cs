using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure;
using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.EquipmentServices.Dtos.Qkny.PowerOnParam;
using Hymson.MES.Core.Constants;
using static Dapper.SqlMapper;

namespace Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam
{
    /// <summary>
    /// 开机参数
    /// </summary>
    public class ProcEquipmentGroupParamService : IProcEquipmentGroupParamService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备参数组 仓储
        /// </summary>
        private readonly IProcEquipmentGroupParamRepository _procEquipmentGroupParamRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcEquipmentGroupParamService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcEquipmentGroupParamRepository procEquipmentGroupParamRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEquipmentGroupParamRepository = procEquipmentGroupParamRepository;
        }

        /// <summary>
        /// 获取设备开机参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<List<ProcEquipmentGroupParamEquProductView>> QueryByEquProductAsync(
            ProcEquipmentGroupParamEquProductQuery query)
        {
            query.Type = ((int)EquipmentGroupParamTypeEnum.OpenParam).ToString();
            var list = await _procEquipmentGroupParamRepository.QueryByEquProductAsync(query);
            if(list == null || list.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45021)).WithData("EquipmentCode", query.EquipmentCode);
            }
            return list;
        }

        /// <summary>
        /// 根据配方获取参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProcEquipmentGroupParamDetailView>> GetDetailByCode(ProcEquipmentGroupParamCodeDetailQuery query)
        {
            var list = await _procEquipmentGroupParamRepository.GetDetailByCode(query);
            if (list == null || list.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45022)).WithData("Code", query.Code);
            }
            return list;
        }

        /// <summary>
        /// 根据编码版本型号获取激活的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamEntity> GetEntityByCodeVersion(ProcEquipmentGroupCheckQuery query)
        {
            var model = await _procEquipmentGroupParamRepository.GetEntityByCodeVersion(query);
            if(model == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45023))
                    .WithData("Code", query.Code).WithData("Version", query.Version).WithData("MaterialCode", query.MaterialCode);
            }
            return model;
        }
    }

}
