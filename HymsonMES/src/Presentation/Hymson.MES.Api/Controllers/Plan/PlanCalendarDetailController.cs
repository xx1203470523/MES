
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan;

/// <summary>
/// 生产日历详情接口
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PlanCalendarDetailController : ControllerBase
{
    #region 引用服务

    private readonly IPlanCalendarDetailService _planCalendarDetailService;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="planCalendarDetailService"></param>
    public PlanCalendarDetailController(IPlanCalendarDetailService planCalendarDetailService)
    {
        _planCalendarDetailService = planCalendarDetailService;
    }

    /// <summary>
    /// 生产日历详情 ; 单条数据验证
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("exist")]
    public async Task<bool> IsExistAsync([FromQuery] PlanCalendarDetailQueryDto query)
    {
        return await _planCalendarDetailService.IsExistAsync(query);
    }

    /// <summary>
    /// 生产日历详情 ; 单条数据查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("one")]
    public async Task<PlanCalendarDetailOutputDto> GetOneAsync([FromQuery] PlanCalendarDetailQueryDto query)
    {
        return await _planCalendarDetailService.GetOneAsync(query);
    }

    /// <summary>
    /// 生产日历详情 ; 分页查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<PagedInfo<PlanCalendarDetailOutputDto>> GetPagedAsync([FromQuery] PlanCalendarDetailPagedQueryDto query)
    {
        return await _planCalendarDetailService.GetPagedAsync(query);
    }

    /// <summary>
    /// 生产日历详情 ; 创建
    /// 描述：
    /// </summary>
    /// <param name="createDto">创建操作数据映射对象</param>
    /// <returns></returns>
    [HttpPost("create")]
    [LogDescription("创建生产日历详情", BusinessType.INSERT)]
    [PermissionDescription("plan:calendar_detail:create")]
    public async Task Create(PlanCalendarDetailDto createDto)
    {
        await _planCalendarDetailService.CreateAsync(createDto);
    }

    /// <summary>
    /// 生产日历详情 ; 更新
    /// 描述：
    /// </summary>
    /// <param name="updateDto">更新操作数据映射对象</param>
    /// <returns></returns>
    [HttpPut("update")]
    [LogDescription("更新生产日历详情", BusinessType.UPDATE)]
    [PermissionDescription("plan:calendar_detail:update")]
    public async Task UpdateAsync(PlanCalendarDetailUpdateDto updateDto)
    {
        await _planCalendarDetailService.UpdateAsync(updateDto);
    }

    /// <summary>
    /// 生产日历详情 ; 删除
    /// 描述：
    /// </summary>
    /// <param name="deleteDto">删除操作数据映射对象</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    [LogDescription("删除生产日历详情", BusinessType.DELETE)]
    [PermissionDescription("plan:calendar_detail:delete")]
    public async Task UpdateAsync(PlanCalendarDetailDeleteDto deleteDto)
    {
        await _planCalendarDetailService.DeleteAsync(deleteDto);
    }
}