using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteVehicleFreight.Command;

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
        Task<int> InsertAsync(InteVehicleFreightStackEntity inteVehiceFreightStackEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteVehicleFreightStackEntity> inteVehiceFreightStackEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteVehicleFreightStackEntity inteVehiceFreightStackEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteVehicleFreightStackEntity> inteVehiceFreightStackEntitys);

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
        Task<InteVehicleFreightStackEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<InteVehicleFreightStackEntity> GetBySFCAsync(InteVehiceFreightStackBySfcQuery query);
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightStackEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightStackEntity>> GetEntitiesAsync(EntityByParentIdsQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteVehiceFreightStackQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQuery inteVehiceFreightStackQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehiceFreightStackPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteVehicleFreightStackEntity>> GetPagedInfoAsync(InteVehiceFreightStackPagedQuery inteVehiceFreightStackPagedQuery);
        #endregion

        #region 顷刻

        /// <summary>
        /// 查询已绑定条码列表
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<InteVehicleFreightStackEntity>> GetBySfcListAsync(InteVehiceSfcListQuery query);

        /// <summary>
        /// 根据托盘id和条码进行删除
        /// </summary>
        /// <param name="commandList"></param>
        /// <returns></returns>
        Task<int> DeleteByVehiceBarCode(List<UpdateVehicleFreightStackCommand> commandList);

        #endregion
    }
}
