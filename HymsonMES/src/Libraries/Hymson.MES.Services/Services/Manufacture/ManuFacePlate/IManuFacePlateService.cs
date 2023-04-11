/*
 *creator: Karl
 *
 *describe: 操作面板    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto;

namespace Hymson.MES.Services.Services.Manufacture
{
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
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateCreateDto"></param>
        /// <returns></returns>
        Task CreateManuFacePlateAsync(ManuFacePlateCreateDto manuFacePlateCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuFacePlateAsync(ManuFacePlateModifyDto manuFacePlateModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuFacePlateAsync(long id);

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

    }
}
