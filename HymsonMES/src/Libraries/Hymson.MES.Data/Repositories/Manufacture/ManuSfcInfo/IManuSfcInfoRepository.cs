/*
 *creator: Karl
 *
 *describe: 条码信息表仓储类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-04-11 02:42:47
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码信息表仓储接口
    /// </summary>
    public interface IManuSfcInfoRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcInfoEntity ManuSfcInfoEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="ManuSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcInfoEntity> ManuSfcInfoEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcInfoEntity ManuSfcInfoEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="ManuSfcInfoEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ManuSfcInfoEntity> ManuSfcInfoEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcId"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetBySFCAsync(long sfcId);

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetBySFCIdsAsync(IEnumerable<long> sfcIds);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfoEntity>> GetManuSfcInfoEntitiesAsync(ManuSfcInfo1Query manuSfcInfo1Query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfo1PagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcInfoEntity>> GetPagedInfoAsync(ManuSfcInfo1PagedQuery manuSfcInfo1PagedQuery);

        /// <summary>
        /// 批量更新 是否在用
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        Task<int> UpdatesIsUsedAsync(ManuSfcInfoUpdateIsUsedCommand manuSfcInfoUpdateIsUsedCommand);
        #endregion


        /// <summary>
        /// 车间作业控制 报表分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WorkshopJobControlReportView>> GetPagedInfoWorkshopJobControlReportAsync(WorkshopJobControlReportPagedQuery pageQuery);

        /// <summary>
        /// 根据SFC获取已经使用的
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcInfoEntity> GetUsedBySFCAsync(string sfc);
    }
}
