using Microsoft.AspNetCore.Mvc;
using Hymson.Web.Framework.Attributes;
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using IdGen;

namespace Hymson.MES.Api.Controllers.Manufacture;


[ApiController]
[Route("api/v1/[controller]")]
public class ManuSfcCirculationController : ControllerBase
{
    private readonly IManuSfcCirculationService _manuSfcCirculationService;

    public ManuSfcCirculationController(IManuSfcCirculationService manuSfcCirculationService)
    {
        _manuSfcCirculationService = manuSfcCirculationService;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("pageinfo")]
    public async Task<PagedInfo<ManuSfcCirculationViewDto>> GetPageInfoAsync([FromQuery]ManuSfcCirculationPagedQueryDto pageQueryDto)
    {
        return await _manuSfcCirculationService.GetPageInfoAsync(pageQueryDto); 
    }

    /// <summary>
    /// 增加条码绑定关系
    /// </summary>
    /// <param name="modifyDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("bindSfc")]
    public async Task BindSfcAsync(ManuSfcCirculationBindDto modifyDto)
    {
        await _manuSfcCirculationService.CreateManuSfcCirculationAsync(modifyDto);
    }

    /// <summary>
    /// 根据Id删除绑定关系
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("del/{id}")]
    public async Task DelAsync(long id) 
    {
         await _manuSfcCirculationService.DeteleteManuSfcCirculationAsync(id);
    }

    /// <summary>
    /// 条码流转表替换
    /// </summary>
    /// <param name="updateDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("replace")]
    public async Task UpdateSfcAsync([FromBody]ManuSfcCirculationBindDto updateDto)
    {
        await _manuSfcCirculationService.UpdateManuSfcCirculationAsync(updateDto);
    }
}
