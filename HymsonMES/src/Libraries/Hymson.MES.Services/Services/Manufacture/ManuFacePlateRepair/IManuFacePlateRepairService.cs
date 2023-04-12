/*
 *creator: Karl
 *
 *describe: 在制品维修    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-04-12 10:32:46
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品维修 service接口
    /// </summary>
    public interface IManuFacePlateRepairService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuFacePlateRepairPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateRepairDto>> GetPagedListAsync(ManuFacePlateRepairPagedQueryDto manuFacePlateRepairPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateRepairCreateDto"></param>
        /// <returns></returns>
        Task CreateManuFacePlateRepairAsync(ManuFacePlateRepairCreateDto manuFacePlateRepairCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateRepairModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuFacePlateRepairAsync(ManuFacePlateRepairModifyDto manuFacePlateRepairModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuFacePlateRepairAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuFacePlateRepairAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuFacePlateRepairDto> QueryManuFacePlateRepairByIdAsync(long id);
    }
}
