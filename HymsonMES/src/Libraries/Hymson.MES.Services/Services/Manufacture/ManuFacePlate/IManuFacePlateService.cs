using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture;

/// <summary>
/// 操作面板 service接口
/// </summary>
public interface IManuFacePlateService
{
    /// <summary>
    /// 获取分页List
    /// </summary>
    /// <param name="manuFacePlatePagedQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<ManuFacePlateDto>> GetPagedListAsync(ManuFacePlatePagedQueryDto manuFacePlatePagedQueryDto);

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeletesManuFacePlateAsync(long[] ids);

    /// <summary>
    /// 根据ID查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ManuFacePlateQueryDto> QueryManuFacePlateByIdAsync(long id);

    /// <summary>
    /// 根据Code查询
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<ManuFacePlateQueryDto> QueryManuFacePlateByCodeAsync(string code);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="addManuFacePlateDto"></param>
    /// <returns></returns>
    Task AddManuFacePlateAsync(AddManuFacePlateDto addManuFacePlateDto);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="updateManuFacePlateDto"></param>
    /// <returns></returns>
    Task UpdateManuFacePlateAsync(UpdateManuFacePlateDto updateManuFacePlateDto);

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task UpdateManuFacePlateStatusAsync(ChangeStatusDto param);

    /// <summary>
    /// 在容器编码上确认
    /// </summary>
    /// <param name="manuFacePlateConfirmByContainerCode"></param>
    /// <returns></returns>
    Task<ManuFacePlateContainerOutputDto> PackContainerAsync(ManuFacePlatePackDto manuFacePlateConfirmByContainerCode);
    Task<ManuFacePlateContainerOutputDto> GetContainerInfoListByContainerCodeAsync(ManuFacePlatePackDto input);
    Task<ManuFacePlateContainerRemoveOutputDto> RemoveContainerPackAsync(ManuFacePlateRemovePackedDto input);
    Task ClosePackContainerAsync(ManuFacePlateCloseContainerDto input);
    Task<ManuFacePlateContainerRemoveOutputDto> RemoveAllContainerPackAsync(ManuFacePlateRemoveAllPackedDto input);
    Task OpenPackContainerAsync(ManuFacePlateOpenContainerDto input);
}
