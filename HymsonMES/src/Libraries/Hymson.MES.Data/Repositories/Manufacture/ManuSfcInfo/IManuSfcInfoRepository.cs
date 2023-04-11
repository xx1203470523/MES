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
        /// <param name="manuSfcInfo1Entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcInfo1Entity manuSfcInfo1Entity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcInfo1Entitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcInfo1Entity> manuSfcInfo1Entitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcInfo1Entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcInfo1Entity manuSfcInfo1Entity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcInfo1Entitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcInfo1Entity> manuSfcInfo1Entitys);

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
        Task<ManuSfcInfo1Entity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfo1Entity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcInfo1Entity>> GetManuSfcInfo1EntitiesAsync(ManuSfcInfo1Query manuSfcInfo1Query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfo1PagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcInfo1Entity>> GetPagedInfoAsync(ManuSfcInfo1PagedQuery manuSfcInfo1PagedQuery);
        #endregion
    }
}
