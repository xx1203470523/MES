/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码 service接口
    /// </summary>
    public interface IManuSfcStepMaterialService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuSfcStepMaterialPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepMaterialDto>> GetPagedListAsync(ManuSfcStepMaterialPagedQueryDto manuSfcStepMaterialPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepMaterialCreateDto"></param>
        /// <returns></returns>
        Task CreateManuSfcStepMaterialAsync(ManuSfcStepMaterialCreateDto manuSfcStepMaterialCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcStepMaterialModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuSfcStepMaterialAsync(ManuSfcStepMaterialModifyDto manuSfcStepMaterialModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuSfcStepMaterialAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuSfcStepMaterialAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcStepMaterialDto> QueryManuSfcStepMaterialByIdAsync(long id);
    }
}
