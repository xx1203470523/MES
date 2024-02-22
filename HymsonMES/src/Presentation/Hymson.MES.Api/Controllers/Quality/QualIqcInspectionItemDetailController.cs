
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Qual;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Qual;

/// <summary>
/// <para>@描述：IQC检验项目详情; 接口</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class QualIqcInspectionItemDetailController : ControllerBase
{
    #region 引用服务

    private readonly IQualIqcInspectionItemDetailService _qualIqcInspectionItemDetailService;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualIqcInspectionItemDetailService"></param>
    public QualIqcInspectionItemDetailController(IQualIqcInspectionItemDetailService qualIqcInspectionItemDetailService)
    {
        _qualIqcInspectionItemDetailService = qualIqcInspectionItemDetailService;
    }

    /// <summary>
    /// IQC检验项目详情 ; 单条数据验证
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("exist")]
    public async Task<bool> IsExistAsync([FromQuery] QualIqcInspectionItemDetailQueryDto query)
    {
        return await _qualIqcInspectionItemDetailService.IsExistAsync(query);
    }

    /// <summary>
    /// IQC检验项目详情 ; 单条数据查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("one")]
    public async Task<QualIqcInspectionItemDetailOutputDto> GetOneAsync([FromQuery] QualIqcInspectionItemDetailQueryDto query)
    {
        return await _qualIqcInspectionItemDetailService.GetOneAsync(query);
    }

    /// <summary>
    /// IQC检验项目详情 ; 单条数据查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<IEnumerable<QualIqcInspectionItemDetailOutputDto>> GetListAsync([FromQuery] QualIqcInspectionItemDetailQueryDto query)
    {
        return await _qualIqcInspectionItemDetailService.GetListAsync(query);
    }

    /// <summary>
    /// IQC检验项目详情 ; 分页查询
    /// 描述：
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<PagedInfo<QualIqcInspectionItemDetailOutputDto>> GetPagedAsync([FromQuery] QualIqcInspectionItemDetailPagedQueryDto query)
    {
        return await _qualIqcInspectionItemDetailService.GetPagedAsync(query);
    }

    /// <summary>
    /// IQC检验项目详情 ; 创建
    /// 描述：
    /// </summary>
    /// <param name="createDto">创建操作数据映射对象</param>
    /// <returns></returns>
    [HttpPost("create")]
    [LogDescription("创建IQC检验项目详情", BusinessType.INSERT)]
    [PermissionDescription("qual:iqc_inspection_item_detail:create")]
    public async Task Create(QualIqcInspectionItemDetailDto createDto)
    {
        await _qualIqcInspectionItemDetailService.CreateAsync(createDto);
    }

    /// <summary>
    /// IQC检验项目详情 ; 更新
    /// 描述：
    /// </summary>
    /// <param name="updateDto">更新操作数据映射对象</param>
    /// <returns></returns>
    [HttpPut("update")]
    [LogDescription("更新IQC检验项目详情", BusinessType.UPDATE)]
    [PermissionDescription("qual:iqc_inspection_item_detail:update")]
    public async Task UpdateAsync(QualIqcInspectionItemDetailUpdateDto updateDto)
    {
        await _qualIqcInspectionItemDetailService.UpdateAsync(updateDto);
    }

    /// <summary>
    /// IQC检验项目详情 ; 删除
    /// 描述：
    /// </summary>
    /// <param name="deleteDto">删除操作数据映射对象</param>
    /// <returns></returns>
    [HttpDelete("delete")]
    [LogDescription("删除IQC检验项目详情", BusinessType.DELETE)]
    [PermissionDescription("qual:iqc_inspection_item_detail:delete")]
    public async Task UpdateAsync(QualIqcInspectionItemDetailDeleteDto deleteDto)
    {
        await _qualIqcInspectionItemDetailService.DeleteAsync(deleteDto);
    }
}