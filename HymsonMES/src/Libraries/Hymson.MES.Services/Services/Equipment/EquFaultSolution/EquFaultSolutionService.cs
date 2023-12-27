using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（设备故障解决措施） 
    /// </summary>
    public class EquFaultSolutionService : IEquFaultSolutionService
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
        private readonly AbstractValidator<EquFaultSolutionSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备故障解决措施）
        /// </summary>
        private readonly IEquFaultSolutionRepository _equFaultSolutionRepository;

        /// <summary>
        /// 仓储接口（设备故障原因） 
        /// </summary>
        private readonly IEquFaultReasonRepository _equFaultReasonRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equFaultSolutionRepository"></param>
        /// <param name="equFaultReasonRepository"></param>
        /// <param name="localizationService"></param>
        public EquFaultSolutionService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultSolutionSaveDto> validationSaveRules,
            IEquFaultSolutionRepository equFaultSolutionRepository,
            IEquFaultReasonRepository equFaultReasonRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equFaultSolutionRepository = equFaultSolutionRepository;
            _equFaultReasonRepository = equFaultReasonRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultSolutionSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultSolutionEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.Status = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _equFaultSolutionRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12900)).WithData("Code", entity.Code);

            // 保存
            return await _equFaultSolutionRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultSolutionSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultSolutionEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            // 编码唯一性验证
            var checkEntity = await _equFaultSolutionRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }

            return await _equFaultSolutionRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equFaultSolutionRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultSolutionDto>> GetPagedListAsync(EquFaultSolutionPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquFaultSolutionPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equFaultSolutionRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultSolutionDto>());
            return new PagedInfo<EquFaultSolutionDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultSolutionDto?> QueryByIdAsync(long id)
        {
            var entity = await _equFaultSolutionRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return entity.ToModel<EquFaultSolutionDto>();
        }

        /// <summary>
        /// 根据ID获取关联故障原因
        /// </summary>
        /// <param name="solutionId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BaseInfoDto>> QueryReasonsByMainIdAsync(long solutionId)
        {
            if (solutionId == 0) return Array.Empty<BaseInfoDto>();

            var relationEntities = await _equFaultSolutionRepository.GetReasonRelationEntitiesAsync(new EntityByParentIdQuery { ParentId = solutionId });
            if (relationEntities == null || !relationEntities.Any()) return Array.Empty<BaseInfoDto>();

            var reasonEntities = await _equFaultReasonRepository.GetByIdsAsync(relationEntities.Select(s => s.FaultReasonId));
            if (reasonEntities == null || !reasonEntities.Any()) return Array.Empty<BaseInfoDto>();

            return reasonEntities.Select(s => new BaseInfoDto
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name
            });
        }

        /// <summary>
        /// 获取解决措施列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QuerySolutionsAsync()
        {
            var solutionEntities = await _equFaultSolutionRepository.GetEntitiesAsync(new EntityByStatusQuery { SiteId = _currentSite.SiteId ?? 0 });
            return solutionEntities.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"{s.Code} - {s.Name}",
                Value = $"{s.Id}"
            });
        }



        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto statusDto)
        {
            if (statusDto.Id == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10125));

            // 校验状态是否在枚举里面
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), statusDto.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            // 不能重新设置为新建
            if (statusDto.Status == SysDataStatusEnum.Build) throw new CustomerValidationException(nameof(ErrorCode.MES10128));

            // 读取当前实时信息
            var entity = await _equFaultSolutionRepository.GetByIdAsync(statusDto.Id);
            if (entity == null || entity.IsDeleted != 0) throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            if (entity.Status == statusDto.Status) throw new CustomerValidationException(nameof(ErrorCode.MES10127))
                    .WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));

            await _equFaultSolutionRepository.UpdateStatusAsync(new ChangeStatusCommand()
            {
                Id = statusDto.Id,
                Status = statusDto.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            });
        }

        #endregion

    }
}
