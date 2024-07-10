using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表仓储接口
    /// </summary>
    public partial interface IManuSfcStepRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcStepEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcStepQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetManuSfcStepEntitiesAsync(ManuSfcStepQuery manuSfcStepQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoAsync(ManuSfcStepPagedQuery manuSfcStepPagedQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcStepEntity manuSfcStepEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcStepEntity>? manuSfcStepEntities);

        /// <summary>
        /// 批量新增-不分表
        /// </summary>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        Task<int> InsertRangeMavleAsync(IEnumerable<ManuSfcStepEntity>? manuSfcStepEntities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcStepEntity manuSfcStepEntity);



        /// <summary>
        /// 插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntitie"></param>
        /// <returns></returns>
        Task<int> InsertSfcStepBusinessAsync(MaunSfcStepBusinessEntity maunSfcStepBusinessEntitie);

        /// <summary>
        /// 批量插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntities"></param>
        /// <returns></returns>
        Task<int> InsertSfcStepBusinessRangeAsync(IEnumerable<MaunSfcStepBusinessEntity> maunSfcStepBusinessEntities);

        /// <summary>
        /// 分页查询 根据SFC
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoBySFCAsync(ManuSfcStepBySfcPagedQuery queryParam);

        /// <summary>
        /// 获取一些条码的所有进站信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetSFCInStepAsync(SfcInStepQuery query);

        /// <summary>
        /// 根据实体列表对数据进行按表名分组
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Dictionary<string, IGrouping<string, ManuSfcStepEntity>> GetTableNames(IEnumerable<ManuSfcStepEntity> entities);

        /// <summary>
        /// 获取SFC的进站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetInStationStepsBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 获取SFC的进站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
         Task<IEnumerable<ManuSfcStepEntity>> GetStepsBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 获取SFC的出站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetOutStationStepsBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 获取SFC的进出站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetInOutStationStepsBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 指定表情查询条码的进出站步骤
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetInOutStationStepsBySFCsAsync(string tableName, EntityBySFCsQuery query);

        /// <summary>
        /// 根据条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcStepEntity>> GetProductParameterBySFCEntitiesAsync(EntityBySFCsQuery param);

        /// <summary>
        /// 获取一个条码的合并新增或拆分新增步骤记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcStepEntity> GetSfcMergeOrSplitAddStepAsync(SfcMergeOrSplitAddStepQuery query);

        Task<ManuSfcStepEntity> GetBarcodeBindingStepAsync(SfcMergeOrSplitAddStepQuery query);
    }
}
