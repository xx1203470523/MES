
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Qual;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Qual;

/// <summary>
/// <para>@描述：IQC检验项目; 接口</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class QualIqcInspectionItemController : ControllerBase
{
    #region 引用服务

    private readonly IQualIqcInspectionItemService _qualIqcInspectionItemService;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualIqcInspectionItemService"></param>
    public QualIqcInspectionItemController(IQualIqcInspectionItemService qualIqcInspectionItemService)
    {
        _qualIqcInspectionItemService = qualIqcInspectionItemService;
    }

    /// <summary>
    /// IQC检验项目 ; 单条数据验证
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("exist")]
    public async Task<bool> IsExistAsync([FromQuery] QualIqcInspectionItemQueryDto query)
    {
        return await _qualIqcInspectionItemService.IsExistAsync(query);
    }

    /// <summary>
    /// IQC检验项目 ; 单条数据查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("one")]
    public async Task<QualIqcInspectionItemOutputDto> GetOneAsync([FromQuery] QualIqcInspectionItemQueryDto query)
    {
        return await _qualIqcInspectionItemService.GetOneAsync(query);
    }

    /// <summary>
    /// IQC检验项目 ; 分页查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<PagedInfo<QualIqcInspectionItemOutputDto>> GetPagedAsync([FromQuery] QualIqcInspectionItemPagedQueryDto query)
    {
        return await _qualIqcInspectionItemService.GetPagedAsync(query);
    }

    /// <summary>
    /// IQC检验项目 ; 创建
    /// 描述：
    /// </summary>
    /// <param name="createDto">创建操作数据映射对象</param>
    /// <returns></returns>
    [HttpPost("create")]
    [LogDescription("创建IQC检验项目", BusinessType.INSERT)]
    [PermissionDescription("qual:iqc_inspection_item:create")]
    public async Task Create(QualIqcInspectionItemDto createDto)
    {
        await _qualIqcInspectionItemService.CreateAsync(createDto);
    }

    /// <summary>
    /// IQC检验项目 ; 更新
    /// 描述：
    /// </summary>
    /// <param name="updateDto">更新操作数据映射对象</param>
    /// <returns></returns>
    [HttpPut("update")]
    [LogDescription("更新IQC检验项目", BusinessType.UPDATE)]
    [PermissionDescription("qual:iqc_inspection_item:update")]
    public async Task UpdateAsync(QualIqcInspectionItemUpdateDto updateDto)
    {
        await _qualIqcInspectionItemService.UpdateAsync(updateDto);
    }

    /// <summary>
    /// IQC检验项目 ; 删除
    /// 描述：
    /// </summary>
    /// <param name="deleteDto">删除操作数据映射对象</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    [LogDescription("删除IQC检验项目", BusinessType.DELETE)]
    [PermissionDescription("qual:iqc_inspection_item:delete")]
    public async Task UpdateAsync(QualIqcInspectionItemDeleteDto deleteDto)
    {
        await _qualIqcInspectionItemService.DeleteAsync(deleteDto);
    }
}