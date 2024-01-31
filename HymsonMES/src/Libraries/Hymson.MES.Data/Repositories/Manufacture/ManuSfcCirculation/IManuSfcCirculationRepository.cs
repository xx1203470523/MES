using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码流转表仓储接口
    /// </summary>
    public interface IManuSfcCirculationRepository
    {
        #region 查询

        /// <summary>
        /// 单条数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcCirculationEntity> GetOneAsync(ManuSfcCirculationQuery query);

        /// <summary>
        /// 数据集查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetListAsync(ManuSfcCirculationQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcCirculationEntity>> GetPagedInfoAsync(ManuSfcCirculationPagedQuery query);

        #endregion

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcCirculationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationQuery query);

        /// <summary>
        /// 根据SFCs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns> 
        Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationBySfcsQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcCirculationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationEntitiesAsync(ManuSfcCirculationQuery manuSfcCirculationQuery);

        /// <summary>
        /// 根据流转前和流转后条码获取绑定记录
        /// </summary>
        /// <param name="manuSfcCirculationBarCodeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationBarCodeEntitiesAsync(ManuSfcCirculationBarCodeQuery manuSfcCirculationBarCodeQuery);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcCirculationEntity manuSfcCirculationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcCirculationEntity manuSfcCirculationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 在制品拆解移除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DisassemblyUpdateAsync(DisassemblyCommand command);

        /// <summary>
        /// 组件使用报告 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcCirculationEntity>> GetReportPagedInfoAsync(ComUsageReportPagedQuery queryParam);

        /// <summary>
        /// 追溯 分页查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcCirculationEntity>> GetProductTraceReportPagedInfoAsync(ProductTraceReportPagedQuery queryParam);

        /// <summary>
        /// 替换条码
        /// </summary>
        /// <param name="bind"></param>
        /// <returns></returns>
        Task<int> UpdateSfcAsync(ManuSfcCirculationBind bind);
    }
}
