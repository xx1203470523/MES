/*
 *creator: Karl
 *
 *describe: 容器条码表仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 容器条码表仓储接口
    /// </summary>
    public interface IManuContainerBarcodeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuContainerBarcodeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuContainerBarcodeEntity> manuContainerBarcodeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuContainerBarcodeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuContainerBarcodeEntity> manuContainerBarcodeEntitys);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="manuContainerBarcodeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ManuContainerBarcodeEntity manuContainerBarcodeEntity);

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
        Task<ManuContainerBarcodeEntity> GetByIdAsync(long id);
        /// <summary>
        /// 根据code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuContainerBarcodeEntity> GetByCodeAsync(ManuContainerBarcodeQuery query);

        /// <summary>
        /// 根据code批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerBarcodeEntity>> GetByCodesAsync(ManuContainerBarcodeQuery query);

        /// <summary>
        /// 根据productId获取数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="status">容器状态 1 打开 ，2关闭</param>
        /// <returns></returns>
        Task<ManuContainerBarcodeEntity> GetByProductIdAsync(long pid, int status, int level);

        /// <summary>
        /// 根据物料编码获取数据
        /// </summary>
        /// <param name="materialCode">物料编码</param>
        /// <param name="status">容器状态 1 打开 ，2关闭</param>
        /// <param name="level">包装等级</param>
        /// <returns></returns>
        Task<ManuContainerBarcodeEntity> GetByMaterialCodeAsync(string materialCode, int status, int level);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerBarcodeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuContainerBarcodeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuContainerBarcodeEntity>> GetManuContainerBarcodeEntitiesAsync(ManuContainerBarcodeQuery manuContainerBarcodeQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerBarcodeQueryView>> GetPagedInfoAsync(ManuContainerBarcodePagedQuery query);

        Task<PagedInfo<ManuContainerBarcodeEntity>> GetPagedListAsync(ManuContainerBarcodePagedQuery manuContainerBarcodePagedQuery);
        Task<ManuContainerBarcodeEntity> GetOneAsync(ManuContainerBarcodeQuery query);
        Task<IEnumerable<ManuContainerBarcodeEntity>> GetListAsync(ManuContainerBarcodeQuery query);
        Task<int> IncrementQtyAsync(IncrementQtyCommand command);
        Task<int> ChangeContainerStatusAsync(CloseContainerCommand command);
        Task<int> InsertReAsync(ManuContainerBarcodeEntity entity);
        Task<int> RefreshQtyAsync(RefreshQtyCommand command);
        Task<int> ClearQtyAsync(ClearQtyCommand command);
        Task<int> RefreshStatusAsync(RefreshStatusCommand command);

        Task<IEnumerable<ManuContainerBarcodeEntity>> GetByContainerIdAsync(ManuContainerIdQuery query);
        #endregion
    }
}
