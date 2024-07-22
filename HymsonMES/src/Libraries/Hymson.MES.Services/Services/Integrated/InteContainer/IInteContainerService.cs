using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Inte;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.InteContainer;

/// <summary>
/// 服务接口（容器维护）
/// </summary>
public interface IInteContainerService
{
    /// <summary>
    /// 添加（容器维护）
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    Task<int> CreateAsync(InteContainerInfoDto createDto);

    /// <summary>
    /// 更新（容器维护）
    /// </summary>
    /// <param name="modifyDto"></param>
    /// <returns></returns>
    Task<int> ModifyAsync(InteContainerInfoUpdateDto modifyDto);

    /// <summary>
    /// 删除（容器维护）
    /// </summary>
    /// <param name="idsArr"></param>
    /// <returns></returns>
    Task<int> DeletesAsync(IEnumerable<long> idsArr);

    /// <summary>
    /// 根据ID获取Info数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Dtos.Inte.InteContainerInfoOutputDto> GetInfoByIdAsync(long id);

    /// <summary>
    /// 根据容器ID获取容器规格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<InteContainerSpecificationOutputDto> GetSpecificationByIdAsync(long id);

    /// <summary>
    /// 查询Freight列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<InteContainerFreightOutputDto>> GetContainerFreightInfoByIdAsync(long id);

    /// <summary>
    /// 获取分页数据（容器维护）
    /// </summary>
    /// <param name="pagedQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<InteContainerInfoOutputDto>> GetPagedListAsync(InteContainerInfoPagedQueryDto pagedQueryDto);

    /// <summary>
    /// 查询详情（容器维护）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<InteContainerReDto> GetDetailAsync(long id);

    /// <summary>
    /// 状态变更
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task UpdateStatusAsync(ChangeStatusDto param);


    /// <summary>
    /// 根据容器编码查询信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<InteContainerInfoViewDto> QueryContainerInfoByCodeAsync(InteContainerQueryDto queryDto);
}
