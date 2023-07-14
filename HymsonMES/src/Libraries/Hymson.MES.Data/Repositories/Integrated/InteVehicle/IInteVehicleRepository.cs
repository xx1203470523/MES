/*
 *creator: Karl
 *
 *describe: 载具注册表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具注册表仓储接口
    /// </summary>
    public interface IInteVehicleRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteVehicleEntity inteVehicleEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleEntity> inteVehicleEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleEntity inteVehicleEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehicleEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleEntity> inteVehicleEntitys);

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
        Task<InteVehicleEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehicleQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleEntity>> GetInteVehicleEntitiesAsync(InteVehicleQuery inteVehicleQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleView>> GetPagedInfoAsync(InteVehiclePagedQuery query);
        #endregion

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteVehicleEntity> GetByCodeAsync(InteVehicleCodeQuery query);
    }
}
