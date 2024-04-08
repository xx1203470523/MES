using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap.Command;
using Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap.View;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap
{
    /// <summary>
    /// 物料报废仓储接口
    /// </summary>
    public interface IWhMaterialInventoryScrapRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryScrapEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<WhMaterialInventoryScrapEntity> whMaterialInventoryScrapEntity);

        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="whMaterialInventoryScrapEntity"></param>
        /// <returns></returns>
        Task<int> InsertSingleAsync(WhMaterialInventoryScrapEntity whMaterialInventoryScrapEntity);
        /// <summary>
        /// 批量更新物料报废是否取消状态
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<int> UpdateIncreaseQuantityResidueRangeAsync(IEnumerable<UpdateCancellationCommand> updateCancellationCommand);

        /// <summary>
        /// 根据条码查询物料信息
        /// </summary>
        /// <param name="MaterialCode"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryScrapView>> GetMaterialInventoryScrapByMaterialCodeAsync(MaterialScrappingCommand command);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryScrapEntity>> GetPagedInfoAsync(WhMaterialInventoryScrapPagedQuery dto);

        /// <summary>
        /// 根据物料ID获取报废信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryScrapEntity>> GetByMaterialIdsAsync(IEnumerable<long> ids);

    }
}
