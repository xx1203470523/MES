
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan;

/// <summary>
/// 生产日历接口
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PlanCalendarController : ControllerBase
{
    #region 引用服务

    private readonly IPlanCalendarService _planCalendarService;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="planCalendarService"></param>
    public PlanCalendarController(IPlanCalendarService planCalendarService)
    {
        _planCalendarService = planCalendarService;
    }

    /// <summary>
    /// 生产日历 ; 单条数据验证
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("exist")]
    public async Task<bool> IsExistAsync([FromQuery] PlanCalendarQueryDto query)
    {
        return await _planCalendarService.IsExistAsync(query);
    }

    /// <summary>
    /// 生产日历 ; 单条数据查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("one")]
    public async Task<PlanCalendarOutputDto> GetOneAsync([FromQuery] PlanCalendarQueryDto query)
    {
        return await _planCalendarService.GetOneAsync(query);
    }

    /// <summary>
    /// 生产日历 ; 分页查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<PagedInfo<PlanCalendarOutputDto>> GetPagedAsync([FromQuery] PlanCalendarPagedQueryDto query)
    {
        return await _planCalendarService.GetPagedAsync(query);
    }

    /// <summary>
    /// 生产日历 ; 创建
    /// 描述：
    /// </summary>
    /// <param name="createDto">创建操作数据映射对象</param>
    /// <returns></returns>
    [HttpPost("create")]
    [LogDescription("创建生产日历", BusinessType.INSERT)]
    [PermissionDescription("plan:calendar:create")]
    public async Task Create(PlanCalendarDto createDto)
    {
        await _planCalendarService.CreateAsync(createDto);
    }

    /// <summary>
    /// 生产日历 ; 更新
    /// 描述：
    /// </summary>
    /// <param name="updateDto">更新操作数据映射对象</param>
    /// <returns></returns>
    [HttpPut("update")]
    [LogDescription("更新生产日历", BusinessType.UPDATE)]
    [PermissionDescription("plan:calendar:update")]
    public async Task UpdateAsync(PlanCalendarUpdateDto updateDto)
    {
        await _planCalendarService.UpdateAsync(updateDto);
    }

    /// <summary>
    /// 生产日历 ; 删除
    /// 描述：
    /// </summary>
    /// <param name="deleteDto">删除操作数据映射对象</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    [LogDescription("删除生产日历", BusinessType.DELETE)]
    [PermissionDescription("plan:calendar:delete")]
    public async Task UpdateAsync(PlanCalendarDeleteDto deleteDto)
    {
        await _planCalendarService.DeleteAsync(deleteDto);
    }
}