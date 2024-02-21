
using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.Services.Qual;

/// <summary>
/// <para>@描述：IQC检验项目详情; 服务</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemDetailService : IQualIqcInspectionItemDetailService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    #region 仓储

    /// <summary>
    /// IQC检验项目详情; 仓储
    /// </summary>
    private readonly IQualIqcInspectionItemDetailRepository _qualIqcInspectionItemDetailRepository;

    private readonly IProcParameterRepository _procParameterRepository;

    #endregion

    #region 验证器

    /// <summary>
    /// 创建验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemDetailDto> _validationCreateRules;

    /// <summary>
    /// 更新验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemDetailUpdateDto> _validationUpdateRules;

    /// <summary>
    /// 删除验证器
    /// </summary>
    private readonly AbstractValidator<QualIqcInspectionItemDetailDeleteDto> _validationDeleteRules;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="currentSite"></param>
    /// <param name="qualIqcInspectionItemDetailRepository"></param>
    /// <param name="validationCreateRules"></param>
    /// <param name="validationUpdateRules"></param>
    /// <param name="validationDeleteRules"></param>
    /// <param name="procParameterRepository"></param>
    public QualIqcInspectionItemDetailService(
    ICurrentUser currentUser,
    ICurrentSite currentSite,
    IQualIqcInspectionItemDetailRepository qualIqcInspectionItemDetailRepository,
    AbstractValidator<QualIqcInspectionItemDetailDto> validationCreateRules,
    AbstractValidator<QualIqcInspectionItemDetailUpdateDto> validationUpdateRules,
    AbstractValidator<QualIqcInspectionItemDetailDeleteDto> validationDeleteRules,
    IProcParameterRepository procParameterRepository)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;

        _qualIqcInspectionItemDetailRepository = qualIqcInspectionItemDetailRepository;

        _validationCreateRules = validationCreateRules;
        _validationUpdateRules = validationUpdateRules;
        _validationDeleteRules = validationDeleteRules;
        _procParameterRepository = procParameterRepository;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<QualIqcInspectionItemDetailOutputDto>> GetPagedAsync(QualIqcInspectionItemDetailPagedQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemDetailPagedQuery>();
        query.SiteId = _currentSite.SiteId;

        var result = new PagedInfo<QualIqcInspectionItemDetailOutputDto>(Enumerable.Empty<QualIqcInspectionItemDetailOutputDto>(), query.PageIndex, query.PageSize);

        var pageResult = await _qualIqcInspectionItemDetailRepository.GetPagedInfoAsync(query);
        if (pageResult.Data != null && pageResult.Data.Any())
        {
            result.Data = pageResult.Data.Select(m => m.ToModel<QualIqcInspectionItemDetailOutputDto>());
            result.TotalCount = pageResult.TotalCount;
        }

        return result;
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualIqcInspectionItemDetailOutputDto>> GetListAsync(QualIqcInspectionItemDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemDetailEntities = await _qualIqcInspectionItemDetailRepository.GetListAsync(query);
        if (qualIqcInspectionItemDetailEntities == null || !qualIqcInspectionItemDetailEntities.Any())
        {
            return Enumerable.Empty<QualIqcInspectionItemDetailOutputDto>();
        }

        var parameterIds = qualIqcInspectionItemDetailEntities.Select(m => m.ParameterId);
        var parameterEntities = await _procParameterRepository.GetByIdsAsync(parameterIds);

        var result = qualIqcInspectionItemDetailEntities.Select(m =>
        {
            var item = m.ToModel<QualIqcInspectionItemDetailOutputDto>();

            var parameterEntity = parameterEntities.FirstOrDefault(e => e.Id == m.ParameterId);
            if (parameterEntity != null)
            {
                item.ParameterCode = parameterEntity.ParameterCode;
                item.ParameterName = parameterEntity.ParameterName;
                item.ParameterUnit = parameterEntity.ParameterUnit;
            }
            return item;
        });

        return result;
        
    }

    /// <summary>
    /// 获取单行数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<QualIqcInspectionItemDetailOutputDto> GetOneAsync(QualIqcInspectionItemDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemDetailEntity = await _qualIqcInspectionItemDetailRepository.GetOneAsync(query);
        if (qualIqcInspectionItemDetailEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        var result = qualIqcInspectionItemDetailEntity.ToModel<QualIqcInspectionItemDetailOutputDto>();
        if (result == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        return result;
    }

    /// <summary>
    /// 检测数据是否存在
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<bool> IsExistAsync(QualIqcInspectionItemDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<QualIqcInspectionItemDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var qualIqcInspectionItemDetailEntity = await _qualIqcInspectionItemDetailRepository.GetOneAsync(query);
        if (qualIqcInspectionItemDetailEntity == null)
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
    public async Task CreateAsync(QualIqcInspectionItemDetailDto createDto)
    {
        await _validationCreateRules.ValidateAndThrowAsync(createDto);

        var command = createDto.ToCommand<QualIqcInspectionItemDetailCreateCommand>();
        command.Init();
        command.CreatedBy = _currentUser.UserName;
        command.UpdatedBy = _currentUser.UserName;
        command.SiteId = _currentSite.SiteId;

        await _qualIqcInspectionItemDetailRepository.InsertAsync(command);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    public async Task UpdateAsync(QualIqcInspectionItemDetailUpdateDto updateDto)
    {
        await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

        var command = updateDto.ToCommand<QualIqcInspectionItemDetailUpdateCommand>();
        command.Init();
        command.UpdatedBy = _currentUser.UserName;

        await _qualIqcInspectionItemDetailRepository.UpdateAsync(command);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="deleteDto"></param>
    /// <returns></returns>
    public async Task DeleteAsync(QualIqcInspectionItemDetailDeleteDto deleteDto)
    {
        await _validationDeleteRules.ValidateAndThrowAsync(deleteDto);

        var command = new DeleteCommand { Ids = deleteDto.Ids };

        await _qualIqcInspectionItemDetailRepository.DeleteMoreAsync(command);
    }
}