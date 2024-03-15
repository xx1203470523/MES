using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组仓储接口
    /// </summary>
    public interface IProcEquipmentGroupParamRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procEquipmentGroupParamQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetProcEquipmentGroupParamEntitiesAsync(ProcEquipmentGroupParamQuery procEquipmentGroupParamQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEquipmentGroupParamView>> GetPagedInfoAsync(ProcEquipmentGroupParamPagedQuery query);

        /// <summary>
        /// 根据关联信息（产品，工序，工艺组）获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetByRelatesInformationAsync(ProcEquipmentGroupParamRelatesInformationQuery query);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        #region 顷刻
        /// <summary>
        /// 根据设备ID和产品型号查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcEquipmentGroupParamEquProductView>> QueryByEquProductAsync(ProcEquipmentGroupParamEquProductQuery query);

        /// <summary>
        /// 根据编码获取激活的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcEquipmentGroupParamDetailView>> GetDetailByCode(ProcEquipmentGroupParamCodeDetailQuery query);

        /// <summary>
        /// 根据编码版本型号获取激活的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamEntity> GetEntityByCodeVersion(ProcEquipmentGroupCheckQuery query);
        #endregion
    }
}
