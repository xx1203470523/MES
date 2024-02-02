
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

namespace Hymson.MES.Services.Plan;

/// <summary>
/// 基础信息 - 仓库服务
/// </summary>
public class PlanCalendarDetailService : IPlanCalendarDetailService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    #region 仓储

    /// <summary>
    /// 生产日历详情; 仓储
    /// </summary>
    private readonly IPlanCalendarDetailRepository _planCalendarDetailRepository;

    #endregion

    #region 验证器

    /// <summary>
    /// 创建验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarDetailDto> _validationCreateRules;

    /// <summary>
    /// 更新验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarDetailUpdateDto> _validationUpdateRules;

    /// <summary>
    /// 删除验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarDetailDeleteDto> _validationDeleteRules;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="currentSite"></param>
    /// <param name="planCalendarDetailRepository"></param>
    /// <param name="validationCreateRules"></param>
    /// <param name="validationUpdateRules"></param>
    /// <param name="validationDeleteRules"></param>
    public PlanCalendarDetailService(
    ICurrentUser currentUser,
    ICurrentSite currentSite,
    IPlanCalendarDetailRepository planCalendarDetailRepository,
    AbstractValidator<PlanCalendarDetailDto> validationCreateRules,
    AbstractValidator<PlanCalendarDetailUpdateDto> validationUpdateRules,
    AbstractValidator<PlanCalendarDetailDeleteDto> validationDeleteRules)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;

        _planCalendarDetailRepository = planCalendarDetailRepository;

        _validationCreateRules = validationCreateRules;
        _validationUpdateRules = validationUpdateRules;
        _validationDeleteRules = validationDeleteRules;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<PlanCalendarDetailOutputDto>> GetPagedAsync(PlanCalendarDetailPagedQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarDetailPagedQuery>();
        query.SiteId = _currentSite.SiteId;

        var result = new PagedInfo<PlanCalendarDetailOutputDto>(Enumerable.Empty<PlanCalendarDetailOutputDto>(), query.PageIndex, query.PageSize);

        var pageResult = await _planCalendarDetailRepository.GetPagedInfoAsync(query);
        if (pageResult.Data != null && pageResult.Data.Any())
        {
            result.Data = pageResult.Data.Select(m => m.ToModel<PlanCalendarDetailOutputDto>());
            result.TotalCount = pageResult.TotalCount;
        }

        return result;
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PlanCalendarDetailOutputDto>> GetListAsync(PlanCalendarDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarDetailEntities = await _planCalendarDetailRepository.GetListAsync(query);
        if (planCalendarDetailEntities == null || !planCalendarDetailEntities.Any())
        {
            return Enumerable.Empty<PlanCalendarDetailOutputDto>();
        }

        return planCalendarDetailEntities.Select(m => m.ToModel<PlanCalendarDetailOutputDto>());
    }

    /// <summary>
    /// 获取单行数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<PlanCalendarDetailOutputDto> GetOneAsync(PlanCalendarDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarDetailEntity = await _planCalendarDetailRepository.GetOneAsync(query);
        if (planCalendarDetailEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        var result = planCalendarDetailEntity.ToModel<PlanCalendarDetailOutputDto>();
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
    public async Task<bool> IsExistAsync(PlanCalendarDetailQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarDetailQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarDetailEntity = await _planCalendarDetailRepository.GetOneAsync(query);
        if (planCalendarDetailEntity == null)
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
    public async Task CreateAsync(PlanCalendarDetailDto createDto)
    {
        await _validationCreateRules.ValidateAndThrowAsync(createDto);

        var command = createDto.ToCommand<PlanCalendarDetailCreateCommand>();
        command.CreatedBy = _currentUser.UserName;
        command.CreatedOn = HymsonClock.Now();
        command.UpdatedBy = _currentUser.UserName;
        command.UpdatedOn = command.CreatedOn;
        command.SiteId = _currentSite.SiteId;
        command.IsDeleted = 0;

        await _planCalendarDetailRepository.InsertAsync(command);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PlanCalendarDetailUpdateDto updateDto)
    {
        await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

        var command = updateDto.ToCommand<PlanCalendarDetailUpdateCommand>();
        command.UpdatedBy = _currentUser.UserName;
        command.UpdatedOn = HymsonClock.Now();

        await _planCalendarDetailRepository.UpdateAsync(command);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="deleteDto"></param>
    /// <returns></returns>
    public async Task DeleteAsync(PlanCalendarDetailDeleteDto deleteDto)
    {
        await _validationDeleteRules.ValidateAndThrowAsync(deleteDto);

        var command = new DeleteCommand { Ids = deleteDto.Ids };

        await _planCalendarDetailRepository.DeleteMoreAsync(command);
    }
}