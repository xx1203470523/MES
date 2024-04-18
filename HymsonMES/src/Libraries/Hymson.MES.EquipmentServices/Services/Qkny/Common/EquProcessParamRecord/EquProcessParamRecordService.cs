using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.CcdFileUploadCompleteRecord;
using Hymson.MES.Core.Domain.EquProcessParamRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquProcessParamRecord;
using Hymson.MES.Data.Repositories.EquProcessParamRecord.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.EquProcessParamRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquProcessParamRecord
{
    /// <summary>
    /// 服务（过程参数记录表） 
    /// </summary>
    public class EquProcessParamRecordService : IEquProcessParamRecordService
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
        private readonly AbstractValidator<EquProcessParamRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（过程参数记录表）
        /// </summary>
        private readonly IEquProcessParamRecordRepository _equProcessParamRecordRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquProcessParamRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquProcessParamRecordSaveDto> validationSaveRules, 
            IEquProcessParamRecordRepository equProcessParamRecordRepository,
            IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equProcessParamRecordRepository = equProcessParamRecordRepository;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<EquProcessParamRecordSaveDto> saveDtoList)
        {
            //1. 查询参数是否存在，存在则录入id
            var paramList = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = saveDtoList.First().SiteId,
                Codes = saveDtoList.Select(x => x.ParamCode)
            });

            List<EquProcessParamRecordEntity> list = saveDtoList
                .Select(m => new EquProcessParamRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentId = m.EquipmentId,
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
            return await _equProcessParamRecordRepository.InsertRangeAsync(list);
        }
    }
}
