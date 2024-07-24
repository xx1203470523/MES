using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Inte;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Inte;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Data;

namespace Hymson.MES.Services.Services.Integrated.InteContainer;

/// <summary>
/// 服务（容器维护）
/// </summary>
public partial class InteContainerService : IInteContainerService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    private readonly IInteContainerRepository _inteContainerRepository;
    private readonly IInteContainerInfoRepository _inteContainerInfoRepository;
    private readonly IInteContainerSpecificationRepository _inteContainerSpecificationRepository;
    private readonly IInteContainerFreightRepository _inteContainerFreightRepository;

    private readonly IProcMaterialRepository _procMaterialRepository;

    private readonly ILocalizationService _localizationService;

    private readonly AbstractValidator<InteContainerInfoDto> _validationContainerInfoCreateRules;
    private readonly AbstractValidator<InteContainerInfoUpdateDto> _validationContainerInfoUpdateRules;

    public InteContainerService(ICurrentUser currentUser,
        ICurrentSite currentSite,
        IInteContainerRepository inteContainerRepository,
        IInteContainerSpecificationRepository inteContainerSpecificationRepository,
        IInteContainerFreightRepository inteContainerFreightRepository,
        IProcMaterialRepository procMaterialRepository,
        ILocalizationService localizationService,
        AbstractValidator<InteContainerInfoDto> validationContainerInfoCreateRules,
        AbstractValidator<InteContainerInfoUpdateDto> validationContainerInfoUpdateRules,
        IInteContainerInfoRepository inteContainerInfoRepository)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;
        _inteContainerRepository = inteContainerRepository;
        _inteContainerSpecificationRepository = inteContainerSpecificationRepository;
        _inteContainerFreightRepository = inteContainerFreightRepository;
        _procMaterialRepository = procMaterialRepository;
        _localizationService = localizationService;
        _validationContainerInfoCreateRules = validationContainerInfoCreateRules;
        _validationContainerInfoUpdateRules = validationContainerInfoUpdateRules;
        _inteContainerInfoRepository = inteContainerInfoRepository;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    public async Task<int> CreateAsync(InteContainerInfoDto createDto)
    {
        // 验证DTO
        await _validationContainerInfoCreateRules.ValidateAndThrowAsync(createDto);

        var now = HymsonClock.Now();
        var userName = _currentUser.UserName;
        var siteId = _currentSite.SiteId ?? 0;

        // DTO转换实体

        var entity = createDto.ToEntity<InteContainerInfoEntity>();

        entity.Id = IdGenProvider.Instance.CreateId();
        entity.CreatedOn = now;
        entity.CreatedBy = userName;
        entity.UpdatedOn = now;
        entity.UpdatedBy = userName;
        entity.SiteId = siteId;

        entity.Status = SysDataStatusEnum.Build;

        // 编码唯一性验证

        var checkEntity = await _inteContainerRepository.GetByCodeAsync(new EntityByCodeQuery
        {
            Site = entity.SiteId,
            Code = entity.Code
        });

        if (checkEntity != null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
        }


        // 验证是否相同物料或者物料组已经设置过
        //var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
        //{
        //    DefinitionMethod = entity.DefinitionMethod,
        //    MaterialId = entity.MaterialId,
        //    MaterialGroupId = entity.MaterialGroupId,
        //    Level = entity.Level
        //});

        //if (entityByRelation != null) throw new CustomerValidationException(nameof(ErrorCode.MES12503));

        //Dto转换实体

        if (createDto.ContainerSpecification == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10100)).WithData("Code", entity.Code);
        }

        var saveContainerSpecificationEntity = createDto.ContainerSpecification.ToEntity<InteContainerSpecificationEntity>();

        //验证参数规格

        ////是否有参数数据
        //bool isSpecificationEntityParametersAnyNull = new[]
        //{
        //    saveContainerSpecificationEntity.Height,
        //    saveContainerSpecificationEntity.Length,
        //    saveContainerSpecificationEntity.Weight,
        //    saveContainerSpecificationEntity.MaxFillWeight,
        //    saveContainerSpecificationEntity.Width
        //}.Any(x => x == null);

        //if (isSpecificationEntityParametersAnyNull)
        //{
        //    throw new CustomerValidationException(nameof(ErrorCode.MES12514));
        //}

        saveContainerSpecificationEntity.Id = IdGenProvider.Instance.CreateId();
        saveContainerSpecificationEntity.ContainerId = entity.Id;
        saveContainerSpecificationEntity.CreatedBy = userName;
        saveContainerSpecificationEntity.CreatedOn = now;
        saveContainerSpecificationEntity.UpdatedBy = userName;
        saveContainerSpecificationEntity.UpdatedOn = now;
        saveContainerSpecificationEntity.SiteId = siteId;


        //关联容器货物

        if (createDto.FreightGroups == null)
        {
            createDto.FreightGroups = new List<InteContainerFreightDto>();
        }

        var freightGroupEntities = createDto.FreightGroups.Select(s =>
        {
            var freightEntity = s.ToEntity<InteContainerFreightEntity>();
            freightEntity.Id = IdGenProvider.Instance.CreateId();
            freightEntity.ContainerId = entity.Id;
            freightEntity.Type = s.Type.GetValueOrDefault();
            freightEntity.Minimum = s.Minimum;
            freightEntity.Maximum = s.Maximum;
            freightEntity.SiteId = siteId;
            freightEntity.Version = s.Version ?? "";
            freightEntity.LevelValue = s.LevelValue;
            freightEntity.CreatedBy = userName;
            freightEntity.CreatedOn = now;
            freightEntity.UpdatedBy = userName;
            freightEntity.UpdatedOn = now;
            return freightEntity;
        });
        //验证关联货物信息

        //是否有相同的物料
        var freightGroupEntitiesGroups = freightGroupEntities.Where(m => m.Type == Core.Enums.Integrated.ContainerFreightTypeEnum.Material).GroupBy(m => new { m.Version, m.LevelValue });
        foreach(var group in freightGroupEntitiesGroups)
        {
            if(group.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12503));
            }
        }

        //是否有相同的容器
        bool isHaveSameContainer = freightGroupEntities.Where(dto => dto.FreightContainerId != null).GroupBy(dto => dto.FreightContainerId).Any(g => g.Count() > 1);
        if (isHaveSameContainer)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12515));
        }

        //是否有相同的物料组
        bool isHaveSameMaterialGroup = freightGroupEntities.Where(dto => dto.MaterialGroupId != null).GroupBy(dto => dto.MaterialGroupId).Any(g => g.Count() > 1);
        if (isHaveSameMaterialGroup)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12503));
        }

        //最大最小必填
        bool isHavevalue = freightGroupEntities.All(dto => dto.Maximum != null && dto.Minimum != null);
        if (!isHavevalue)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12516));
        }

        //最大值是否大于最小值
        bool isMaxiGreaterMini = freightGroupEntities.All(dto => dto.Maximum > dto.Minimum);
        if (!isMaxiGreaterMini)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12502));
        }
        ////规格参数都大于0
        //bool allSpecificationGreaterZero = new[]
        //    {   saveContainerSpecificationEntity.Height,
        //        saveContainerSpecificationEntity.Length,
        //        saveContainerSpecificationEntity.Weight,
        //        saveContainerSpecificationEntity.MaxFillWeight,
        //        saveContainerSpecificationEntity.Width}.All(x => x > 0);

        //if (!allSpecificationGreaterZero)
        //{
        //    throw new CustomerValidationException(nameof(ErrorCode.MES12514));
        //}

        var rows = 0;

        using var trans = TransactionHelper.GetTransactionScope();

        rows = await _inteContainerRepository.InsertInfoAsync(entity);
        if (rows <= 0)
        {
            trans.Dispose();
        }
        else
        {
            var rowArray = await Task.WhenAll(new List<Task<int>>() {
                    _inteContainerRepository.InsertFreightAsync(freightGroupEntities),
                    _inteContainerRepository.InsertSpecificationAsync(saveContainerSpecificationEntity)
            });
            rows += rowArray.Sum();
            trans.Complete();
        }
        return rows;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="modifyDto"></param>
    /// <returns></returns>
    public async Task<int> ModifyAsync(InteContainerInfoUpdateDto modifyDto)
    {
        // 验证DTO
        await _validationContainerInfoUpdateRules.ValidateAndThrowAsync(modifyDto);

        var updatedOn = HymsonClock.Now();
        var updatedBy = _currentUser.UserName;
        var siteId = _currentSite.SiteId ?? 0;

        var inteContainerEntity = await _inteContainerRepository.GetContainerInfoByIdAsync(modifyDto.Id ?? 0);        

        //验证某些状态是不能编辑的
        var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
        if (!canEditStatusEnum.Any(x => x == inteContainerEntity.Status))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10129));
        }


        // DTO转换实体
        var entity = modifyDto.ToEntity<InteContainerInfoEntity>();
        entity.Status = inteContainerEntity.Status;
        entity.UpdatedBy = updatedBy;
        entity.UpdatedOn = updatedOn;
        //组装容器Freight

        if (modifyDto.FreightGroups == null)
        {
            modifyDto.FreightGroups = Enumerable.Empty<InteContainerFreightUpdateDto>();
        }

        var freightGroupEntities = modifyDto.FreightGroups.Select(s =>
        {
            var freightEntity = s.ToEntity<InteContainerFreightEntity>();
            freightEntity.Id = IdGenProvider.Instance.CreateId();
            freightEntity.ContainerId = entity.Id;
            freightEntity.Type = s.Type.GetValueOrDefault();
            freightEntity.Minimum = s.Minimum;
            freightEntity.Maximum = s.Maximum;
            freightEntity.SiteId = siteId;
            freightEntity.LevelValue = s.LevelValue;
            freightEntity.CreatedBy = updatedBy;
            freightEntity.CreatedOn = updatedOn;
            freightEntity.UpdatedBy = updatedBy;
            freightEntity.UpdatedOn = updatedOn;
            return freightEntity;
        });

        //组装容器参数数据
        var specificationEntity = modifyDto.ContainerSpecification.ToEntity<InteContainerSpecificationEntity>();
        specificationEntity.Id = IdGenProvider.Instance.CreateId();
        specificationEntity.SiteId = siteId;
        specificationEntity.ContainerId = modifyDto.Id;
        specificationEntity.CreatedBy = updatedBy;
        specificationEntity.CreatedOn = updatedOn;
        specificationEntity.UpdatedBy = updatedBy;
        specificationEntity.UpdatedOn = updatedOn;

        ////规格参数都大于0
        //bool allSpecificationGreaterZero = new[]
        //    {   specificationEntity.Height,
        //        specificationEntity.Length,
        //        specificationEntity.Weight,
        //        specificationEntity.MaxFillWeight,
        //        specificationEntity.Width}.All(x => x > 0);

        //if (!allSpecificationGreaterZero)
        //{
        //    throw new CustomerValidationException(nameof(ErrorCode.MES12514));
        //}

        //是否有相同的物料
        bool isHaveSameMaterial = freightGroupEntities.GroupBy(dto => new { dto.Version, dto.LevelValue }).Any(g => g.Count() > 1);
        if (isHaveSameMaterial)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12503));
        }

        //是否有相同的物料组
        bool isHaveSameMaterialGroup = freightGroupEntities.Where(dto => dto.MaterialGroupId != null).GroupBy(dto => dto.MaterialGroupId).Any(g => g.Count() > 1);
        if (isHaveSameMaterialGroup)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12503));
        }

        //是否有相同的容器
        bool isHaveSameContainer = freightGroupEntities.Where(dto => dto.FreightContainerId != null).GroupBy(dto => dto.FreightContainerId).Any(g => g.Count() > 1);
        if (isHaveSameContainer)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12515));
        }

        //最大值是否大于最小值
        bool isMaxiGreaterMini = freightGroupEntities.All(dto => dto.Maximum > dto.Minimum);
        if (!isMaxiGreaterMini)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12502));
        }

        var command = new DeleteByParentIdCommand
        {
            ParentId = modifyDto.Id ?? 0,
            UpdatedBy = updatedBy,
            UpdatedOn = updatedOn
        };

        //删除参数、容器

        var rows = 0;
        using (var trans = TransactionHelper.GetTransactionScope())
        {
            await _inteContainerRepository.DeleteByParentIdAsync(command);
            await _inteContainerRepository.DeleteSpecificationByParentIdAsync(command);
            await _inteContainerRepository.UpdateAsync(entity);
            var rowArray = await Task.WhenAll(new List<Task<int>>()
            {
                _inteContainerRepository.InsertFreightAsync(freightGroupEntities),
                   _inteContainerRepository.InsertSpecificationAsync(specificationEntity)
            });
            rows += rowArray.Sum();
            trans.Complete();
        }
        return rows;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="idsArr"></param>
    /// <returns></returns>
    public async Task<int> DeletesAsync(IEnumerable<long>  idsArr)
    {
        var list = await _inteContainerRepository.GetByIdsAsync(idsArr);
        if (list != null && list.Any(x => x.Status != SysDataStatusEnum.Build))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12509));
        }
        return await _inteContainerRepository.DeletesAsync(new DeleteCommand
        {
            Ids = idsArr,
            UserId = _currentUser.UserName,
            DeleteOn = HymsonClock.Now()
        });
    }

    /// <summary>
    /// 根据ID获取Info数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<InteContainerInfoOutputDto> GetInfoByIdAsync(long id)
    {
        var entity = await _inteContainerRepository.GetContainerInfoByIdAsync(id);
        InteContainerInfoOutputDto dto = new InteContainerInfoOutputDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            Remark = entity.Remark
        };
        return dto;
    }

    /// <summary>
    /// 根据容器ID获取容器规格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<InteContainerSpecificationOutputDto> GetSpecificationByIdAsync(long id)
    {
        var dto = await _inteContainerRepository.GetSpecificationAsync(id);
        InteContainerSpecificationOutputDto inteContainerSpecificationGroupsDto = new InteContainerSpecificationOutputDto
        {
            Height = dto.Height,
            Width = dto.Width,
            Length = dto.Length,
            MaxFillWeight = dto.MaxFillWeight,
            Weight = dto.Weight
        };
        return inteContainerSpecificationGroupsDto;
    }

    /// <summary>
    /// 查询容器货物
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<InteContainerFreightOutputDto>> GetContainerFreightInfoByIdAsync(long id)
    {
        //var inteFreightEntities = await _inteContainerRepository.GetFreightAsync(new EntityByParentIdQuery
        //{
        //    ParentId = id,
        //    SiteId = _currentSite.SiteId.GetValueOrDefault()
        //});

        var inteFreightEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
        {
            ContainerId = id,
            SiteId = _currentSite.SiteId.GetValueOrDefault()
        });

        List<InteContainerFreightOutputDto> list = new List<InteContainerFreightOutputDto>();

        foreach (var item in inteFreightEntities)
        {
            var dto = new InteContainerFreightOutputDto
            {
                LevelValue = item.LevelValue,
                Type = item.Type,
                MaterialId = item.MaterialId,
                MaterialGroupId = item.MaterialGroupId,
                FreightContainerId = item.FreightContainerId,
                Minimum = item.Minimum,
                Maximum = item.Maximum
            };
            list.Add(dto);
        }

        var materialFreights = list.Where(m => m.Type == Core.Enums.Integrated.ContainerFreightTypeEnum.Material);
        if(materialFreights.Any())
        {
            var materialIds = materialFreights.Select(x => x.MaterialId.GetValueOrDefault());

            var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds);
            
            foreach(var materialFreightItem in materialFreights)
            {
                var materialEntity = materialEntities.FirstOrDefault(m => m.Id == materialFreightItem.MaterialId);
                if( materialEntity != null)
                {
                    materialFreightItem.Version = materialEntity.Version ?? "";
                }
            }
        }

        return list;
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<InteContainerInfoOutputDto>> GetPagedListAsync(InteContainerInfoPagedQueryDto pageQueryDto)
    {
        var siteId = _currentSite.SiteId.GetValueOrDefault();

        var pagedQuery = new InteContainerInfoPagedQuery
        {
            PageIndex = pageQueryDto.PageIndex,
            PageSize = pageQueryDto.PageSize,
            NameLike = pageQueryDto.Name,
            CodeLike = pageQueryDto.Code,
            Status = pageQueryDto.Status,
            SiteId = siteId,
            Sorting = "CreatedOn Desc"
        };

        var result = new List<InteContainerInfoOutputDto>();

        var dbResult = await _inteContainerInfoRepository.GetPagedInfoAsync(pagedQuery);
        if (dbResult.Data != null && dbResult.Data.Any())
        {
            var containerIds = dbResult.Data.Select(m => m.Id);

            var containerSpecificationEntities = await _inteContainerSpecificationRepository.GetListAsync(new InteContainerSpecificationQuery
            {
                ContainerIds = containerIds,
                SiteId = siteId
            });

            var containerFpecificationEntities = await _inteContainerFreightRepository.GetListAsync(new InteContainerFreightQuery
            {
                ContainerIds = containerIds,
                SiteId = siteId
            });

            foreach (var data in dbResult.Data)
            {
                var containerSpecificationEntity = containerSpecificationEntities.FirstOrDefault(m => m.ContainerId == data.Id);
                var _containerFpecificationEntities = containerFpecificationEntities.Where(m => m.ContainerId == data.Id);

                var outputItem = data.ToModel<InteContainerInfoOutputDto>();

                if (containerSpecificationEntity != null)
                {
                    outputItem.ContainerSpecification = containerSpecificationEntity.ToModel<InteContainerSpecificationOutputDto>();
                }

                if (_containerFpecificationEntities != null && _containerFpecificationEntities.Any())
                {
                    outputItem.FreightGroups = _containerFpecificationEntities.Where(m => m.ContainerId == data.Id).Select(m => m.ToModel<InteContainerFreightOutputDto>());
                }

                result.Add(outputItem);
            }
        }

        return new PagedInfo<InteContainerInfoOutputDto>(result, dbResult.PageIndex, dbResult.PageSize, dbResult.TotalCount);
    }

    /// <summary>
    /// 查询详情（容器维护）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<InteContainerReDto> GetDetailAsync(long id)
    {
        var inteContainerEntity = await _inteContainerRepository.GetByIdAsync(id);
        if (inteContainerEntity == null)
        {
            return new InteContainerReDto();
        }

        return inteContainerEntity.ToModel<InteContainerReDto>();
    }

    #region 状态变更

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task UpdateStatusAsync(ChangeStatusDto param)
    {
        #region 参数校验
        if (param.Id == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10125));
        }
        if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10126));
        }
        if (param.Status == SysDataStatusEnum.Build)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10128));
        }

        #endregion

        var changeStatusCommand = new ChangeStatusCommand()
        {
            Id = param.Id,
            Status = param.Status,

            UpdatedBy = _currentUser.UserName,
            UpdatedOn = HymsonClock.Now()
        };

        #region 校验数据
        var entity = await _inteContainerRepository.GetContainerInfoByIdAsync(changeStatusCommand.Id);
        if (entity == null || entity.IsDeleted != 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES12513));
        }
        if (entity.Status == changeStatusCommand.Status)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
        }
        #endregion

        #region 操作数据库
        await _inteContainerRepository.UpdateStatusAsync(changeStatusCommand);
        #endregion
    }

    #endregion
}
