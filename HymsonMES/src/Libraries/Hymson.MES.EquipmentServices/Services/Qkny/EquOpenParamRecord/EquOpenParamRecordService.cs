using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquOpenParamRecord;
using Hymson.MES.Core.Domain.EquProcessParamRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquOpenParamRecord;
using Hymson.MES.Data.Repositories.EquOpenParamRecord.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.EquOpenParamRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquOpenParamRecord
{
    /// <summary>
    /// 服务（开机参数记录表） 
    /// </summary>
    public class EquOpenParamRecordService : IEquOpenParamRecordService
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
        private readonly AbstractValidator<EquOpenParamRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（开机参数记录表）
        /// </summary>
        private readonly IEquOpenParamRecordRepository _equOpenParamRecordRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquOpenParamRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquOpenParamRecordSaveDto> validationSaveRules, 
            IEquOpenParamRecordRepository equOpenParamRecordRepository,
            IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equOpenParamRecordRepository = equOpenParamRecordRepository;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 新增多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<EquOpenParamRecordSaveDto> saveDtoList)
        {
            //1. 查询参数是否存在，存在则录入id
            var paramList = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = saveDtoList.First().SiteId,
                Codes = saveDtoList.Select(x => x.ParamCode)
            });

            List<EquOpenParamRecordEntity> list = saveDtoList
                .Select(m => new EquOpenParamRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentId = m.EquipmentId,
                    ParamCode = m.ParamCode,
                    ParamValue = m.ParamValue,
                    ParamId = paramList.Where(p => p.ParameterCode == m.ParamCode).FirstOrDefault()?.Id,
                    BatchId = m.BatchId,
                    RecipeId = m.RecipeId,
                    CollectionTime = m.CollectionTime,
                    CreatedBy = m.CreatedBy,
                    CreatedOn = m.CreatedOn,
                    UpdatedBy = m.UpdatedBy,
                    UpdatedOn = m.UpdatedOn,
                    IsDeleted = m.IsDeleted,
                    Remark = m.Remark,
                    SiteId = m.SiteId,
                }).ToList();
            return await _equOpenParamRecordRepository.InsertRangeAsync(list);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquOpenParamRecordDto>> GetPagedListAsync(EquOpenParamRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquOpenParamRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equOpenParamRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquOpenParamRecordDto>());
            return new PagedInfo<EquOpenParamRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
