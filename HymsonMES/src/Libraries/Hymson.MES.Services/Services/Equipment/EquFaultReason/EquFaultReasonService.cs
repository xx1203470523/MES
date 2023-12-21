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
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 设备故障原因表 服务
    /// </summary>
    public class EquFaultReasonService : IEquFaultReasonService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EquFaultReasonSaveDto> _validationSaveRules;

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
        /// <param name="equFaultReasonRepository"></param>
        /// <param name="localizationService"></param>
        public EquFaultReasonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultReasonSaveDto> validationSaveRules,
            IEquFaultReasonRepository equFaultReasonRepository,
            ILocalizationService localizationService)
        {
            _currentSite = currentSite;
            _currentUser = currentUser;
            _validationSaveRules = validationSaveRules;
            _equFaultReasonRepository = equFaultReasonRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultReasonSaveDto saveDto)
        {
            // 验证DTO
            saveDto.Code = saveDto.Code.ToTrimSpace();
            saveDto.Code = saveDto.Code.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultReasonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            entity.Status = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _equFaultReasonRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES13011)).WithData("Code", entity.Code);

            // 保存实体
            return await _equFaultReasonRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultReasonSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultReasonEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            // 编码唯一性验证
            var checkEntity = await _equFaultReasonRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }

            return await _equFaultReasonRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _equFaultReasonRepository.GetByIdsAsync(idsArr);
            if (entities != null && entities.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _equFaultReasonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquFaultReasonPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equFaultReasonRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultReasonDto>());
            return new PagedInfo<EquFaultReasonDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultReasonDto> QueryByIdAsync(long id)
        {
            var EquFaultReasonEntity = await _equFaultReasonRepository.GetByIdAsync(id);
            var dto = EquFaultReasonEntity.ToModel<CustomEquFaultReasonDto>();
            return dto;
        }

        /// <summary>
        /// 获取解决措施（可被引用）
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QueryReasonsAsync()
        {
            var solutionEntities = await _equFaultReasonRepository.GetEntitiesAsync(new EntityByStatusQuery { SiteId = _currentSite.SiteId ?? 0 });
            return solutionEntities.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"${s.Code} - ${s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 根据ID获取关联解决措施
        /// </summary>
        /// <param name="phenomenonId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QueryReasonsByMainIdAsync(long phenomenonId)
        {
            if (phenomenonId == 0) return Array.Empty<SelectOptionDto>();

            var relationEntities = await _equFaultReasonRepository.GetRelationEntitiesAsync(new EntityByParentIdQuery { ParentId = phenomenonId });
            if (relationEntities == null || !relationEntities.Any()) return Array.Empty<SelectOptionDto>();

            var solutionEntities = await _equFaultReasonRepository.GetByIdsAsync(relationEntities.Select(s => s.FaultReasonId));
            if (solutionEntities == null || !solutionEntities.Any()) return Array.Empty<SelectOptionDto>();

            return solutionEntities.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"${s.Code} - ${s.Name}",
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
            var entity = await _equFaultReasonRepository.GetByIdAsync(statusDto.Id);
            if (entity == null || entity.IsDeleted != 0) throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            if (entity.Status == statusDto.Status) throw new CustomerValidationException(nameof(ErrorCode.MES10127))
                    .WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));

            await _equFaultReasonRepository.UpdateStatusAsync(new ChangeStatusCommand()
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
