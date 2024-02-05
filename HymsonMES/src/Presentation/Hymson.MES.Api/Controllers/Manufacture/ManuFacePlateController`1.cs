using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture;

public partial class ManuFacePlateController
{
    /// <summary>
    /// 获取容器信息列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("container/list")]
    public async Task<ManuFacePlateContainerOutputDto> GetContainerInfoListByContainerCodeAsync([FromQuery] ManuFacePlatePackDto input)
    {
        return await _manuFacePlateService.GetContainerInfoListByContainerCodeAsync(input);
    }

    /// <summary>
    /// 容器装载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("container/pack")]
    public async Task<ManuFacePlateContainerOutputDto> PackContainerAsync(ManuFacePlatePackDto input)
    {
        return await _manuFacePlateService.PackContainerAsync(input);
    }

    /// <summary>
    /// 容器打开
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("container/open")]
    public async Task OpenPackContainerAsync(ManuFacePlateOpenContainerDto input)
    {
        await _manuFacePlateService.OpenPackContainerAsync(input);
    }

    /// <summary>
    /// 容器关闭
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("container/close")]
    public async Task ClosePackContainerAsync(ManuFacePlateCloseContainerDto input)
    {
        await _manuFacePlateService.ClosePackContainerAsync(input);
    }

    /// <summary>
    /// 容器卸载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("container/remove")]
    public async Task<ManuFacePlateContainerRemoveOutputDto> RemoveContainerPackAsync(ManuFacePlateRemovePackedDto input)
    {
        return await _manuFacePlateService.RemoveContainerPackAsync(input);
    }

    /// <summary>
    /// 容器全部卸载
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("container/remove/all")]
    public async Task<ManuFacePlateContainerRemoveOutputDto> RemoveAllContainerPackAsync(ManuFacePlateRemoveAllPackedDto input)
    {
        return await _manuFacePlateService.RemoveAllContainerPackAsync(input);
    }
}