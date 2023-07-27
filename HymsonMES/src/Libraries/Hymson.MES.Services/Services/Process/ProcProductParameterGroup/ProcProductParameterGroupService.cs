using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

/// <summary>
/// 服务（产品检验参数组） 
/// </summary>
public class ProcProductParameterGroupService : IProcProductParameterGroupService
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
    private readonly AbstractValidator<ProcProductParameterGroupSaveDto> _validationSaveRules;

    /// <summary>
    /// 仓储接口（全检参数表）
    /// </summary>
    private readonly IProcProductParameterGroupRepository _procProductParameterGroupRepository;

    /// <summary>
    /// 仓储接口（全检参数项目表）
    /// </summary>
    private readonly IProcProductParameterGroupDetailRepository _procProductParameterGroupDetailRepository;

    /// <summary>
    /// 仓储接口（物料）
    /// </summary>
    /// </summary>
    private readonly IProcMaterialRepository _procMaterialRepository;

    /// <summary>
    /// 仓储接口（工序）
    /// </summary>
    /// </summary>
    private readonly IProcProcedureRepository _procProcedureRepository;

    /// <summary>
    /// 仓储接口（标准参数）
    /// </summary>
    private readonly IProcParameterRepository _procParameterRepository;

    /// <summary>
    /// 国际化服务
    /// </summary>
    private readonly ILocalizationService _localizationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="currentSite"></param>
    /// <param name="validationSaveRules"></param>
    /// <param name="procProductParameterGroupRepository"></param>
    /// <param name="procProductParameterGroupDetailRepository"></param>
    /// <param name="procMaterialRepository"></param>
    /// <param name="procProcedureRepository"></param>
    /// <param name="procParameterRepository"></param>
    /// <param name="localizationService"></param>
    public ProcProductParameterGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcProductParameterGroupSaveDto> validationSaveRules,
        IProcProductParameterGroupRepository procProductParameterGroupRepository,
        IProcProductParameterGroupDetailRepository procProductParameterGroupDetailRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcParameterRepository procParameterRepository,
        ILocalizationService localizationService)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;
        _validationSaveRules = validationSaveRules;
        _procProductParameterGroupRepository = procProductParameterGroupRepository;
        _procProductParameterGroupDetailRepository = procProductParameterGroupDetailRepository;
        _procMaterialRepository = procMaterialRepository;
        _procProcedureRepository = procProcedureRepository;
        _procParameterRepository = procParameterRepository;
        _localizationService = localizationService;
    }


    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="saveDto"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(ProcProductParameterGroupSaveDto saveDto)
    {
        // 判断是否有获取到站点码 
        if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

        // 验证DTO
        await _validationSaveRules.ValidateAndThrowAsync(saveDto);

        // 更新时间
        var updatedBy = _currentUser.UserName;
        var updatedOn = HymsonClock.Now();

        // DTO转换实体
        var entity = saveDto.ToEntity<ProcProductParameterGroupEntity>();
        entity.Id = IdGenProvider.Instance.CreateId();
        entity.CreatedBy = updatedBy;
        entity.CreatedOn = updatedOn;
        entity.UpdatedBy = updatedBy;
        entity.UpdatedOn = updatedOn;
        entity.SiteId = _currentSite.SiteId ?? 0;

        // 编码唯一性验证
        var checkEntity = await _procProductParameterGroupRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = entity.SiteId,
            Code = entity.Code,
            Version = entity.Version
        });
        if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10520)).WithData("Code", entity.Code).WithData("Version", entity.Version);

        // 判断规格上限和规格下限（数据类型为数值）
        List<ValidationFailure> validationFailures = new();
        foreach (var item in saveDto.Details)
        {
            // 如果参数类型为数值，则判断规格上限和规格下限
            if (item.DataType != DataTypeEnum.Numeric) continue;
            if (item.UpperLimit >= item.LowerLimit) continue;

            validationFailures.Add(new ValidationFailure
            {
                FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                        { "CollectionIndex", item.Code },
                        { "Code", item.Code }
                    },
                ErrorCode = nameof(ErrorCode.MES10516)
            });
        }

        // 是否存在错误
        if (validationFailures.Any())
        {
            //throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            throw new ValidationException("", validationFailures);
        }

        var details = saveDto.Details.Select(s =>
        {
            var detailEntity = s.ToEntity<ProcProductParameterGroupDetailEntity>();
            detailEntity.Id = IdGenProvider.Instance.CreateId();
            detailEntity.ParameterGroupId = entity.Id;
            detailEntity.CreatedBy = updatedBy;
            detailEntity.CreatedOn = updatedOn;
            detailEntity.UpdatedBy = updatedBy;
            detailEntity.UpdatedOn = updatedOn;
            detailEntity.SiteId = entity.SiteId;

            return detailEntity;
        });

        var rows = 0;
        using (var trans = TransactionHelper.GetTransactionScope())
        {
            rows += await _procProductParameterGroupRepository.InsertAsync(entity);
            rows += await _procProductParameterGroupDetailRepository.InsertRangeAsync(details);
            trans.Complete();
        }
        return rows;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="saveDto"></param>
    /// <returns></returns>
    public async Task<int> ModifyAsync(ProcProductParameterGroupSaveDto saveDto)
    {
        // 判断是否有获取到站点码 
        if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

        // 验证DTO
        await _validationSaveRules.ValidateAndThrowAsync(saveDto);

        // 更新时间
        var updatedBy = _currentUser.UserName;
        var updatedOn = HymsonClock.Now();

        // DTO转换实体
        var entity = saveDto.ToEntity<ProcProductParameterGroupEntity>();
        entity.UpdatedBy = updatedBy;
        entity.UpdatedOn = updatedOn;
        entity.SiteId = _currentSite.SiteId ?? 0;

        // 编码唯一性验证
        var checkEntity = await _procProductParameterGroupRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = entity.SiteId,
            Code = entity.Code,
            Version = entity.Version
        });
        if (checkEntity != null && checkEntity.Id != entity.Id)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10520)).WithData("Code", entity.Code).WithData("Version", entity.Version);
        }

        // 判断规格上限和规格下限（数据类型为数值）
        List<ValidationFailure> validationFailures = new();
        foreach (var item in saveDto.Details)
        {
            // 如果参数类型为数值，则判断规格上限和规格下限
            if (item.DataType != DataTypeEnum.Numeric) continue;
            if (item.UpperLimit >= item.LowerLimit) continue;

            validationFailures.Add(new ValidationFailure
            {
                FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                        { "CollectionIndex", item.Code },
                        { "Code", item.Code }
                    },
                ErrorCode = nameof(ErrorCode.MES10516)
            });
        }

        // 是否存在错误
        if (validationFailures.Any())
        {
            //throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            throw new ValidationException("", validationFailures);
        }

        var details = saveDto.Details.Select(s =>
        {
            var detailEntity = s.ToEntity<ProcProductParameterGroupDetailEntity>();
            detailEntity.Id = IdGenProvider.Instance.CreateId();
            detailEntity.ParameterGroupId = entity.Id;
            detailEntity.CreatedBy = updatedBy;
            detailEntity.CreatedOn = updatedOn;
            detailEntity.UpdatedBy = updatedBy;
            detailEntity.UpdatedOn = updatedOn;
            detailEntity.SiteId = entity.SiteId;

            return detailEntity;
        });

        var command = new DeleteByParentIdCommand
        {
            ParentId = entity.Id,
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn
        };

        var rows = 0;
        using (var trans = TransactionHelper.GetTransactionScope())
        {
            rows += await _procProductParameterGroupRepository.UpdateAsync(entity);
            rows += await _procProductParameterGroupDetailRepository.DeleteByParentIdAsync(command);
            rows += await _procProductParameterGroupDetailRepository.InsertRangeAsync(details);
            trans.Complete();
        }
        return rows;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(long id)
    {
        return await _procProductParameterGroupRepository.DeleteAsync(id);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeletesAsync(long[] ids)
    {
        return await _procProductParameterGroupRepository.DeletesAsync(new DeleteCommand
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
    public async Task<ProcProductParameterGroupInfoDto?> QueryByIdAsync(long id)
    {
        var qualEnvParameterGroupEntity = await _procProductParameterGroupRepository.GetByIdAsync(id);
        if (qualEnvParameterGroupEntity == null) return null;

        var dto = qualEnvParameterGroupEntity.ToModel<ProcProductParameterGroupInfoDto>();
        if (dto == null) return dto;

        var materialTask = _procMaterialRepository.GetByIdAsync(dto.MaterialId);
        var procedureTask = _procProcedureRepository.GetByIdAsync(dto.ProcedureId);

        var materialEntity = await materialTask;
        var procedureEntity = await procedureTask;

        if (materialEntity != null) dto.MaterialCode = materialEntity.MaterialCode;
        if (procedureEntity != null) dto.ProcedureCode = procedureEntity.Code;

        return dto;
    }

    /// <summary>
    /// 根据ID获取项目明细列表
    /// </summary>
    /// <param name="parameterGroupId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProductParameterGroupDetailDto>> QueryDetailsByParameterGroupIdAsync(long parameterGroupId)
    {
        var details = await _procProductParameterGroupDetailRepository.GetEntitiesAsync(new ProcProductParameterGroupDetailQuery
        {
            ParameterGroupId = parameterGroupId
        });

        // 查询已经缓存的参数实体
        var parameterEntities = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery
        {
            SiteId = _currentSite.SiteId ?? 0
        });

        List<ProcProductParameterGroupDetailDto> dtos = new();
        foreach (var item in details)
        {
            var dto = item.ToModel<ProcProductParameterGroupDetailDto>();
            var parameterEntity = parameterEntities.FirstOrDefault(f => f.Id == item.ParameterId);
            if (dto == null) continue;

            if (parameterEntity != null)
            {
                dto.Code = parameterEntity.ParameterCode;
                dto.Name = parameterEntity.ParameterName;
                dto.Unit = parameterEntity.ParameterUnit;
                dto.DataType = parameterEntity.DataType;
            }

            dtos.Add(dto);
        }

        return dtos;
    }

    /// <summary>
    /// 根据查询条件获取分页数据
    /// </summary>
    /// <param name="pagedQueryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProcProductParameterGroupDto>> GetPagedListAsync(ProcProductParameterGroupPagedQueryDto pagedQueryDto)
    {
        var pagedQuery = pagedQueryDto.ToQuery<ProcProductParameterGroupPagedQuery>();
        pagedQuery.SiteId = _currentSite.SiteId ?? 0;
        var pagedInfo = await _procProductParameterGroupRepository.GetPagedInfoAsync(pagedQuery);

        // 实体到DTO转换 装载数据
        var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcProductParameterGroupDto>());
        return new PagedInfo<ProcProductParameterGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
    }

}
