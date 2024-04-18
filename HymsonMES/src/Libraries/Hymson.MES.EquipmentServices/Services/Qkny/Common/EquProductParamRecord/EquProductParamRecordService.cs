using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquProcessParamRecord;
using Hymson.MES.Core.Domain.EquProductParamRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquProductParamRecord;
using Hymson.MES.Data.Repositories.EquProductParamRecord.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.EquProductParamRecord;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Hymson.MES.Services.Services.EquProductParamRecord
{
    /// <summary>
    /// 服务（产品参数记录表） 
    /// </summary>
    public class EquProductParamRecordService : IEquProductParamRecordService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<EquProductParamRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（产品参数记录表）
        /// </summary>
        private readonly IEquProductParamRecordRepository _equProductParamRecordRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquProductParamRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquProductParamRecordSaveDto> validationSaveRules,
            IEquProductParamRecordRepository equProductParamRecordRepository,
            IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equProductParamRecordRepository = equProductParamRecordRepository;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<EquProductParamRecordSaveDto> saveDtoList)
        {
            if (saveDtoList.IsNullOrEmpty() == true)
            {
                return 0;
            }
            //1. 查询参数是否存在，存在则录入id
            var paramList = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = saveDtoList.First().SiteId,
                Codes = saveDtoList.Select(x => x.ParamCode)
            });

            List<EquProductParamRecordEntity> list = saveDtoList
                .Select(m => new EquProductParamRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentId = m.EquipmentId,
                    Sfc = m.Sfc,
                    ParamCode = m.ParamCode,
                    ParamValue = m.ParamValue,
                    ParamId = paramList.Where(p => p.ParameterCode == m.ParamCode).FirstOrDefault()?.Id,
                    CollectionTime = m.CollectionTime,
                    CreatedBy = m.CreatedBy,
                    CreatedOn = m.CreatedOn,
                    UpdatedBy = m.UpdatedBy,
                    UpdatedOn = m.UpdatedOn,
                    IsDeleted = m.IsDeleted,
                    Remark = m.Remark,
                    SiteId = m.SiteId,
                }).ToList();
            return await _equProductParamRecordRepository.InsertRangeAsync(list);
        }
    }
}
