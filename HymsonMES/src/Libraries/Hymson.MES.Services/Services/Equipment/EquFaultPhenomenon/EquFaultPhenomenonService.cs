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
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon
{
    /// <summary>
    /// 服务（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonService : IEquFaultPhenomenonService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EquFaultPhenomenonSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonRepository _equFaultPhenomenonRepository;

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
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equFaultPhenomenonRepository"></param>
        /// <param name="equFaultReasonRepository"></param>
        /// <param name="localizationService"></param>
        public EquFaultPhenomenonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultPhenomenonSaveDto> validationSaveRules,
            IEquFaultPhenomenonRepository equFaultPhenomenonRepository,
            IEquFaultReasonRepository equFaultReasonRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equFaultPhenomenonRepository = equFaultPhenomenonRepository;
            _equFaultReasonRepository = equFaultReasonRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultPhenomenonSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultPhenomenonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.Status = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _equFaultPhenomenonRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES13011)).WithData("Code", entity.Code);

            // 关联解决措施
            saveDto.ReasonIds ??= new List<long>();
            var relationEntities = saveDto.ReasonIds.Select(s => new EquFaultPhenomenonReasonRelationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                FaultPhenomenonId = entity.Id,
                FaultReasonId = s
            });

            // 保存实体
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _equFaultPhenomenonRepository.InsertAsync(entity);
            rows += await _equFaultPhenomenonRepository.InsertRelationsAsync(relationEntities);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 修改（设备故障现象）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultPhenomenonSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultPhenomenonEntity>();
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

            // 关联解决措施
            saveDto.ReasonIds ??= new List<long>();
            var relationEntities = saveDto.ReasonIds.Select(s => new EquFaultPhenomenonReasonRelationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                FaultPhenomenonId = entity.Id,
                FaultReasonId = s
            });

            // 保存实体
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _equFaultPhenomenonRepository.DeleteByParentIdAsync(new DeleteByParentIdCommand { ParentId = entity.Id });
            rows += await _equFaultPhenomenonRepository.InsertAsync(entity);
            rows += await _equFaultPhenomenonRepository.InsertRelationsAsync(relationEntities);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _equFaultPhenomenonRepository.GetByIdsAsync(idsArr);
            if (entities != null && entities.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _equFaultPhenomenonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 查询列表（设备故障现象）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync(EquFaultPhenomenonPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquFaultPhenomenonPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equFaultPhenomenonRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultPhenomenonDto>());
            return new PagedInfo<EquFaultPhenomenonDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonDto?> QueryByIdAsync(long id)
        {
            var entity = await _equFaultPhenomenonRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return entity.ToModel<EquFaultPhenomenonDto>();
        }

        /// <summary>
        /// 根据ID获取关联故障原因
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> QueryReasonsByMainIdAsync(long id)
        {
            if (id == 0) return Array.Empty<long>();

            var relationEntities = await _equFaultPhenomenonRepository.GetRelationEntitiesAsync(new EntityByParentIdQuery { ParentId = id });
            if (relationEntities == null || !relationEntities.Any()) return Array.Empty<long>();

            var solutionEntities = await _equFaultReasonRepository.GetByIdsAsync(relationEntities.Select(s => s.FaultReasonId));
            if (solutionEntities == null || !solutionEntities.Any()) return Array.Empty<long>();

            return solutionEntities.Select(s => s.Id);
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
            var entity = await _equFaultPhenomenonRepository.GetByIdAsync(statusDto.Id);
            if (entity == null || entity.IsDeleted != 0) throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            if (entity.Status == statusDto.Status) throw new CustomerValidationException(nameof(ErrorCode.MES10127))
                    .WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));

            await _equFaultPhenomenonRepository.UpdateStatusAsync(new ChangeStatusCommand()
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
