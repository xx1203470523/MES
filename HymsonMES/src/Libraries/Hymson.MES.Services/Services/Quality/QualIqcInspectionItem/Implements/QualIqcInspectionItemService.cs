
using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.Utils;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Qual;

/// <summary>
/// <para>@描述：IQC检验项目; 服务</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemService : IQualIqcInspectionItemService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    #region 仓储

    /// <summary>
    /// IQC检验项目; 仓储
    /// </summary>
    private readonly IQualIqcInspectionItemRepository _qualIqcInspectionItemRepository;

    private readonly IQualIqcInspectionItemDetailRepository _qualIqcInspectionItemDetailRepository;

    private readonly IProcMaterialRepository _procMaterialRepository;

    private readonly IWhSupplierRepository _whSupplierRepository;

    #endregion

    #region 验证器

    /// <summary>
    /// 创建验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemDto> _validationCreateRules;

    /// <summary>
    /// 更新验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemUpdateDto> _validationUpdateRules;

    /// <summary>
    /// 删除验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemDeleteDto> _validationDeleteRules;

    #endregion


    public QualIqcInspectionItemService(
        ICurrentUser currentUser, ICurrentSite currentSite,
        IQualIqcInspectionItemRepository qualIqcInspectionItemRepository,
        IQualIqcInspectionItemDetailRepository qualIqcInspectionItemDetailRepository,
        IProcMaterialRepository procMaterialRepository, IWhSupplierRepository whSupplierRepository,
        AbstractValidator<QualIqcInspectionItemDto> validationCreateRules,
        AbstractValidator<QualIqcInspectionItemUpdateDto> validationUpdateRules,
        AbstractValidator<QualIqcInspectionItemDeleteDto> validationDeleteRules)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;
        _qualIqcInspectionItemRepository = qualIqcInspectionItemRepository;
        _qualIqcInspectionItemDetailRepository = qualIqcInspectionItemDetailRepository;
        _procMaterialRepository = procMaterialRepository;
        _whSupplierRepository = whSupplierRepository;
        _validationCreateRules = validationCreateRules;
        _validationUpdateRules = validationUpdateRules;
        _validationDeleteRules = validationDeleteRules;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<QualIqcInspectionItemOutputDto>> GetPagedAsync(QualIqcInspectionItemPagedQueryDto queryDto)
    {
        var _siteId = _currentSite.SiteId;

        var materialIds = Enumerable.Empty<long>();
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialCode = queryDto.MaterialCode });
            materialIds = materialEntities.Select(m => m.Id);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialName))
        {
            var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _siteId, MaterialName = queryDto.MaterialName });
            materialIds = materialIds.Concat(materialEntities.Select(m => m.Id));
        }
        materialIds = materialIds.Distinct();

        var supplierIds = Enumerable.Empty<long>();
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName))
        {
            var supplierEntities = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery { SiteId = _siteId, Name = queryDto.SupplierName });
            supplierIds = supplierEntities.Select(m => m.Id);
        }

        var query = new QualIqcInspectionItemPagedQuery
        {
            CodeLike = queryDto.Code,
            NameLike = queryDto.Name,
            MaterialIds = materialIds,
            SupplierIds = supplierIds,
            Status = queryDto.Status,
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
            SiteId = _siteId
        };

        var result = new PagedInfo<QualIqcInspectionItemOutputDto>(Enumerable.Empty<QualIqcInspectionItemOutputDto>(), query.PageIndex, query.PageSize);

        var pageResult = await _qualIqcInspectionItemRepository.GetPagedInfoAsync(query);
        if (pageResult.Data != null && pageResult.Data.Any())
        {
            result.Data = pageResult.Data.Select(m => m.ToModel<QualIqcInspectionItemOutputDto>());
            result.TotalCount = pageResult.TotalCount;

            var resultMaterialIds = result.Data.Select(m => m.MaterialId.GetValueOrDefault());
            var resultSupplierIds = result.Data.Select(m => m.SupplierId.GetValueOrDefault());

            var materialEntities = await _procMaterialRepository.GetByIdsAsync(resultMaterialIds);
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(resultSupplierIds);

            result.Data = result.Data.Select(m =>
            {
                var materialEntity = materialEntities.FirstOrDefault(e => e.Id == m.MaterialId);
                if (materialEntity != default)
                {
                    m.MaterialCode = materialEntity.MaterialCode;
                    m.MaterialName = materialEntity.MaterialName;
                    m.MaterialUnit = materialEntity.Unit;
                    m.MaterialVersion = materialEntity.Version;
                }

                var supplierEntity = supplierEntities.FirstOrDefault(e => e.Id == m.SupplierId);
                if (supplierEntity != default)
                {
                    m.SupplierName = supplierEntity.Name;
                }

                return m;
            });
        }

        return result;
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualIqcInspectionItemOutputDto>> GetListAsync(QualIqcInspectionItemQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemEntities = await _qualIqcInspectionItemRepository.GetListAsync(query);
        if (qualIqcInspectionItemEntities == null || !qualIqcInspectionItemEntities.Any())
        {
            return Enumerable.Empty<QualIqcInspectionItemOutputDto>();
        }

        return qualIqcInspectionItemEntities.Select(m => m.ToModel<QualIqcInspectionItemOutputDto>());
    }

    /// <summary>
    /// 获取单行数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<QualIqcInspectionItemOutputDto> GetOneAsync(QualIqcInspectionItemQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemEntity = await _qualIqcInspectionItemRepository.GetOneAsync(query);
        if (qualIqcInspectionItemEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        var result = qualIqcInspectionItemEntity.ToModel<QualIqcInspectionItemOutputDto>();
        if (result == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        var materialEntity = await _procMaterialRepository.GetByIdAsync(result.MaterialId.GetValueOrDefault());
        if (materialEntity != null)
        {
            result.MaterialName = materialEntity.MaterialName;
            result.MaterialCode = materialEntity.MaterialCode;
            result.MaterialUnit = materialEntity.Unit;
            result.MaterialVersion = materialEntity.Version;
        }

        var supplierEntity = await _whSupplierRepository.GetByIdAsync(result.SupplierId.GetValueOrDefault());
        if (supplierEntity != null)
        {
            result.SupplierCode = supplierEntity.Code;
            result.SupplierName = supplierEntity.Name;
        }

        return result;
    }

    /// <summary>
    /// 检测数据是否存在
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<bool> IsExistAsync(QualIqcInspectionItemQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemEntity = await _qualIqcInspectionItemRepository.GetOneAsync(query);
        if (qualIqcInspectionItemEntity == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    public async Task CreateAsync(QualIqcInspectionItemDto createDto)
    {
        await _validationCreateRules.ValidateAndThrowAsync(createDto);

        var command = createDto.ToCommand<QualIqcInspectionItemCreateCommand>();
        command.Init();
        command.CreatedBy = _currentUser.UserName;
        command.UpdatedBy = _currentUser.UserName;
        command.SiteId = _currentSite.SiteId;

        var detailCommands = Enumerable.Empty<QualIqcInspectionItemDetailCreateCommand>();
        if (createDto.QualIqcInspectionItemDetailDtos != null && createDto.QualIqcInspectionItemDetailDtos.Any())
        {
            detailCommands = createDto.QualIqcInspectionItemDetailDtos.Select(m =>
            {
                var detailCommand = m.ToCommand<QualIqcInspectionItemDetailCreateCommand>();
                detailCommand.Init();
                detailCommand.QualIqcInspectionItemId = command.Id;
                detailCommand.CreatedBy = command.CreatedBy;
                detailCommand.UpdatedBy = command.UpdatedBy;
                detailCommand.SiteId = command.SiteId;

                return detailCommand;
            });
        }

        using var scope = TransactionHelper.GetTransactionScope();

        var affectedRow = await _qualIqcInspectionItemRepository.InsertIgnoreAsync(command);
        if (affectedRow == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11908));
        }

        await _qualIqcInspectionItemDetailRepository.InsertAsync(detailCommands);

        scope.Complete();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    public async Task UpdateAsync(QualIqcInspectionItemUpdateDto updateDto)
    {
        await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

        var entity = await _qualIqcInspectionItemRepository.GetOneAsync(new QualIqcInspectionItemQuery { Id = updateDto.Id });
        if (entity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES11909));
        }

        var command = updateDto.ToCommand<QualIqcInspectionItemUpdateCommand>();
        command.Init();
        command.SiteId = _currentSite.SiteId;
        command.UpdatedBy = _currentUser.UserName;

        var detailCommands = Enumerable.Empty<QualIqcInspectionItemDetailCreateCommand>();
        if (updateDto.QualIqcInspectionItemDetailDtos != null && updateDto.QualIqcInspectionItemDetailDtos.Any())
        {
            detailCommands = updateDto.QualIqcInspectionItemDetailDtos.Select(m =>
            {
                var detailCommand = m.ToCommand<QualIqcInspectionItemDetailCreateCommand>();
                detailCommand.Init();
                detailCommand.QualIqcInspectionItemId = command.Id;
                detailCommand.CreatedOn = entity.CreatedOn;
                detailCommand.CreatedBy = entity.CreatedBy;
                detailCommand.UpdatedBy = command.UpdatedBy;
                detailCommand.UpdatedOn = command.UpdatedOn;
                detailCommand.SiteId = command.SiteId;

                return detailCommand;
            });
        }

        using var scope = TransactionHelper.GetTransactionScope();

        await _qualIqcInspectionItemDetailRepository.DeleteByQualIqcInspectionItemIdAsync(entity.Id);

        await _qualIqcInspectionItemRepository.UpdateAsync(command);

        await _qualIqcInspectionItemDetailRepository.InsertAsync(detailCommands);

        scope.Complete();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="deleteDto"></param>
    /// <returns></returns>
    public async Task DeleteAsync(QualIqcInspectionItemDeleteDto deleteDto)
    {
        await _validationDeleteRules.ValidateAndThrowAsync(deleteDto);

        var command = new DeleteCommand { Ids = deleteDto.Ids };

        using var scope = TransactionHelper.GetTransactionScope();

        await _qualIqcInspectionItemRepository.DeleteMoreAsync(command);

        await _qualIqcInspectionItemDetailRepository.DeleteByQualIqcInspectionItemIdsAsync(command.Ids);

        scope.Complete();
    }
}