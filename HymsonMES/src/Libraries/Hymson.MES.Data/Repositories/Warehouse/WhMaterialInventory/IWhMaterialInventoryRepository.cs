/*
 *creator: Karl
 *
 *describe: 物料库存仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;

namespace Hymson.MES.Data.Repositories.Warehouse
{
    /// <summary>
    /// 物料库存仓储接口
    /// </summary>
    public interface IWhMaterialInventoryRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhMaterialInventoryEntity whMaterialInventoryEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<WhMaterialInventoryEntity>? whMaterialInventoryEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhMaterialInventoryEntity whMaterialInventoryEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="whMaterialInventoryEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<WhMaterialInventoryEntity> whMaterialInventoryEntitys);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePointByBarCodeAsync(UpdateStatusByBarCodeCommand command);

        /// <summary>
        /// 批量更新更新状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdatePointByBarCodeRangeAsync(IEnumerable<UpdateStatusByBarCodeCommand> commands);

        /// <summary>
        /// 更新状态（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePointByBarCodesAsync(IEnumerable<UpdateStatusByBarCodeCommand> commands);

        /// <summary>
        /// 更新状态（批量--不操作数量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns> 
         Task<int> UpdateStatusByIdsAsync(UpdateStatusByIdCommand command);

        /// <summary>
        /// 更新状态（批量--不操作数量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns> 
        Task<int> UpdateStatusByBarCodesAsync(IEnumerable<UpdateStatusByBarCodeCommand> commands);

        /// <summary>
        /// 更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<int> UpdateIncreaseQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand);

        /// <summary>

        /// 清空库存
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateWhMaterialInventoryEmptyByBarCodeAync(UpdateWhMaterialInventoryEmptyCommand command);

        /// <summary>
        /// 批量清空库存(根据id)
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateWhMaterialInventoryEmptyByIdRangeAync(IEnumerable<UpdateWhMaterialInventoryEmptyByIdCommand> commands);

        /// <summary>
        /// 清空库存(根据id)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateWhMaterialInventoryEmptyByIdAync(UpdateWhMaterialInventoryEmptyByIdCommand command);

        /// <summary>
        /// 批量更新库存数量(增加库存)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<int> UpdateIncreaseQuantityResidueRangeAsync(IEnumerable<UpdateQuantityRangeCommand> updateQuantityCommand);

        /// <summary>
        /// 批量更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        Task<int> UpdateReduceQuantityResidueRangeAsync(IEnumerable<UpdateQuantityRangeCommand> updateQuantityCommand);

        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityRangeCommand Command);

        /// <summary>
        /// 更新库存数量(减少库存)
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        Task<int> UpdateReduceQuantityResidueAsync(UpdateQuantityCommand updateQuantityCommand);

        /// <summary>
        /// 更新库存数量(减少库存)-带库存检查
        /// </summary>
        /// <param name="updateQuantityCommand"></param>
        /// <returns></returns>
        Task<int> UpdateReduceQuantityResidueWithCheckAsync(UpdateQuantityCommand updateQuantityCommand);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryEntity> GetByBarCodeAsync(WhMaterialInventoryBarCodeQuery query);

        /// <summary>
        /// 根据物料条码获取数据（剩余数量大于0的条码）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesOfHasQtyAsync(WhMaterialInventoryBarCodesQuery param);

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesAsync(WhMaterialInventoryBarCodesQuery param);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="whMaterialInventoryQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetWhMaterialInventoryEntitiesAsync(WhMaterialInventoryQuery whMaterialInventoryQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="whMaterialInventoryPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryPageListView>> GetPagedInfoAsync(WhMaterialInventoryPagedQuery whMaterialInventoryPagedQuery);

        /// <summary>
        /// 根据物料编码获取物料数据
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        Task<ProcMaterialInfoView> GetProcMaterialByMaterialCodeAsync(long materialId);

        /// <summary>
        /// 根据物料编码获取供应商信息
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhSupplierInfoView>> GetWhSupplierByMaterialIdAsync(WhSupplierByMaterialCommand command);

        /// <summary>
        /// 修改外部来源库存
        /// </summary>
        /// <param name="whMaterialInventoryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateOutsideWhMaterilInventoryAsync(WhMaterialInventoryEntity whMaterialInventoryEntity);

        /// <summary>
        /// 根据条码修改库存数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateQuantityResidueBySFCsAsync(UpdateQuantityResidueBySfcsCommand command);

        /// <summary>
        /// 部分报废条码修改库存数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> ScrapPartialWhMaterialInventoryByIdAsync(IEnumerable<ScrapPartialWhMaterialInventoryByIdCommand> commands);

        #region 顷刻

        /// <summary>
        /// 根据物料条码获取数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryEntity>> GetByBarCodesNoQtyAsync(WhMaterialInventoryBarCodesQuery param);

        #endregion
    }
}
