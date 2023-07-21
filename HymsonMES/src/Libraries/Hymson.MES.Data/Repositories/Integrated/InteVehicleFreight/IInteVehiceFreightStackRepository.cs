/*
 *creator: Karl
 *
 *describe: 二维载具条码明细仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-19 08:14:38
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 二维载具条码明细仓储接口
    /// </summary>
    public interface IInteVehiceFreightStackRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehiceFreightStackEntity inteVehiceFreightStackEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehiceFreightStackEntity> inteVehiceFreightStackEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehiceFreightStackEntity inteVehiceFreightStackEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehiceFreightStackEntity> inteVehiceFreightStackEntitys);

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
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteVehiceFreightStackEntity> GetByIdAsync(long id);
        Task<InteVehiceFreightStackEntity> GetBySFCAsync(string sfc);
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehiceFreightStackEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehiceFreightStackQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehiceFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQuery inteVehiceFreightStackQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehiceFreightStackPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehiceFreightStackEntity>> GetPagedInfoAsync(InteVehiceFreightStackPagedQuery inteVehiceFreightStackPagedQuery);
        #endregion
    }
}
