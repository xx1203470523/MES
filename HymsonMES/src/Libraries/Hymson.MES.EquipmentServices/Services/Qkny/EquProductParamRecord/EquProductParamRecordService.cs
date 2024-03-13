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

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquProductParamRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquProductParamRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equProductParamRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquProductParamRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquProductParamRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equProductParamRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equProductParamRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equProductParamRecordRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquProductParamRecordDto?> QueryByIdAsync(long id) 
        {
           var equProductParamRecordEntity = await _equProductParamRecordRepository.GetByIdAsync(id);
           if (equProductParamRecordEntity == null) return null;
           
           return equProductParamRecordEntity.ToModel<EquProductParamRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquProductParamRecordDto>> GetPagedListAsync(EquProductParamRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquProductParamRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equProductParamRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquProductParamRecordDto>());
            return new PagedInfo<EquProductParamRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
