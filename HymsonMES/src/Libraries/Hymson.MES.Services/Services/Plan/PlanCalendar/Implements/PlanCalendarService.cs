
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Plan;

/// <summary>
/// 基础信息 - 仓库服务
/// </summary>
public class PlanCalendarService : IPlanCalendarService
{
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentSite _currentSite;

    #region 仓储

    /// <summary>
    /// 生产日历; 仓储
    /// </summary>
    private readonly IPlanCalendarRepository _planCalendarRepository;

    private readonly IPlanCalendarDetailRepository _planCalendarDetailRepository;

    #endregion

    #region 验证器

    /// <summary>
    /// 创建验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarDto> _validationCreateRules;

    /// <summary>
    /// 更新验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarUpdateDto> _validationUpdateRules;

    /// <summary>
    /// 删除验证器
    /// </summary>
    private readonly AbstractValidator<PlanCalendarDeleteDto> _validationDeleteRules;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="currentSite"></param>
    /// <param name="planCalendarRepository"></param>
    /// <param name="validationCreateRules"></param>
    /// <param name="validationUpdateRules"></param>
    /// <param name="validationDeleteRules"></param>
    /// <param name="planCalendarDetailRepository"></param>
    public PlanCalendarService(
    ICurrentUser currentUser,
    ICurrentSite currentSite,
    IPlanCalendarRepository planCalendarRepository,
    AbstractValidator<PlanCalendarDto> validationCreateRules,
    AbstractValidator<PlanCalendarUpdateDto> validationUpdateRules,
    AbstractValidator<PlanCalendarDeleteDto> validationDeleteRules,
    IPlanCalendarDetailRepository planCalendarDetailRepository)
    {
        _currentUser = currentUser;
        _currentSite = currentSite;

        _planCalendarRepository = planCalendarRepository;

        _validationCreateRules = validationCreateRules;
        _validationUpdateRules = validationUpdateRules;
        _validationDeleteRules = validationDeleteRules;
        _planCalendarDetailRepository = planCalendarDetailRepository;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<PlanCalendarOutputDto>> GetPagedAsync(PlanCalendarPagedQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarPagedQuery>();
        query.SiteId = _currentSite.SiteId;

        var result = new PagedInfo<PlanCalendarOutputDto>(Enumerable.Empty<PlanCalendarOutputDto>(), query.PageIndex, query.PageSize);

        var pageResult = await _planCalendarRepository.GetPagedInfoAsync(query);
        if (pageResult.Data != null && pageResult.Data.Any())
        {
            result.Data = pageResult.Data.Select(m => m.ToModel<PlanCalendarOutputDto>());
            result.TotalCount = pageResult.TotalCount;
        }

        return result;
    }

    /// <summary>
    /// 获取数据集
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PlanCalendarOutputDto>> GetListAsync(PlanCalendarQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarEntities = await _planCalendarRepository.GetListAsync(query);
        if (planCalendarEntities == null || !planCalendarEntities.Any())
        {
            return Enumerable.Empty<PlanCalendarOutputDto>();
        }

        return planCalendarEntities.Select(m => m.ToModel<PlanCalendarOutputDto>());
    }

    /// <summary>
    /// 获取单行数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<PlanCalendarOutputDto> GetOneAsync(PlanCalendarQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarEntity = await _planCalendarRepository.GetOneAsync(query);
        if (planCalendarEntity == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }    

        var result = planCalendarEntity.ToModel<PlanCalendarOutputDto>();
        if (result == null)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES10104));
        }

        var planCalendarDetailEntities = await _planCalendarDetailRepository.GetListAsync(new PlanCalendarDetailQuery { PlanCalendarId = planCalendarEntity.Id });
        if (planCalendarDetailEntities != null)
        {
            result.Details = planCalendarDetailEntities.Select(m => m.ToModel<PlanCalendarDetailOutputDto>());
        }

        return result;
    }

    /// <summary>
    /// 检测数据是否存在
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<bool> IsExistAsync(PlanCalendarQueryDto queryDto)
    {
        var query = queryDto.ToQuery<PlanCalendarQuery>();
        query.SiteId = _currentSite.SiteId;

        var planCalendarEntity = await _planCalendarRepository.GetOneAsync(query);
        if (planCalendarEntity == null)
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
    public async Task CreateAsync(PlanCalendarDto createDto)
    {
        await _validationCreateRules.ValidateAndThrowAsync(createDto);

        var command = createDto.ToCommand<PlanCalendarCreateCommand>();
        command.Init();
        command.CreatedBy = _currentUser.UserName;
        command.UpdatedBy = _currentUser.UserName;
        command.SiteId = _currentSite.SiteId;

        var detailCreateCommands = Enumerable.Empty<PlanCalendarDetailCreateCommand>();
        if(createDto.Details != null && createDto.Details.Any())
        {
            detailCreateCommands = createDto.Details.Where(m => m.Day.HasValue && m.ShiftId.HasValue).Select(m =>
            {
                var _command = new PlanCalendarDetailCreateCommand
                {
                    PlanCalendarId = command.Id,
                    Day = m.Day,
                    ShiftId = m.ShiftId,
                    CreatedBy = command.CreatedBy,
                    UpdatedBy = command.UpdatedBy,
                    SiteId = command.SiteId,
                };
                _command.Init();

                return _command;
            });
        }

        using var scope = TransactionHelper.GetTransactionScope();

        var count = await _planCalendarRepository.InsertIgnoreAsync(command);
        if(count == 0)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES19801));
        }

        if(detailCreateCommands.Any())
        {
            await _planCalendarDetailRepository.InsertAsync(detailCreateCommands);
        }

        scope.Complete();
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PlanCalendarUpdateDto updateDto)
    {
        await _validationUpdateRules.ValidateAndThrowAsync(updateDto);

        var command = updateDto.ToCommand<PlanCalendarUpdateCommand>();
        command.Init();        
        command.SiteId = _currentSite.SiteId;
        command.UpdatedBy = _currentUser.UserName;

        var detailCreateCommands = Enumerable.Empty<PlanCalendarDetailCreateCommand>();
        if (updateDto.Details != null && updateDto.Details.Any())
        {
            detailCreateCommands = updateDto.Details.Where(m => m.Day.HasValue && m.ShiftId.HasValue).Select(m =>
            {
                var _command = new PlanCalendarDetailCreateCommand
                {
                    PlanCalendarId = command.Id,
                    Day = m.Day,
                    ShiftId = m.ShiftId,
                    CreatedBy = command.UpdatedBy,
                    UpdatedBy = command.UpdatedBy,
                    SiteId = command.SiteId,
                };
                _command.Init();

                return _command;
            });
        }

        using var scope = TransactionHelper.GetTransactionScope();

        await _planCalendarDetailRepository.DeleteByPlanCalendarIdAsync(command.Id.GetValueOrDefault());

        var count = await _planCalendarRepository.UpdateAsync(command);
        if (count > 0 && detailCreateCommands.Any())
        {
            await _planCalendarDetailRepository.InsertAsync(detailCreateCommands);
        }        

        scope.Complete();
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="deleteDto"></param>
    /// <returns></returns>
    public async Task DeleteAsync(PlanCalendarDeleteDto deleteDto)
    {
        await _validationDeleteRules.ValidateAndThrowAsync(deleteDto);

        var command = new DeleteCommand { Ids = deleteDto.Ids };

        await _planCalendarRepository.DeleteMoreAsync(command);
    }
}