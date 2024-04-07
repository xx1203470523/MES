using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrapNgRelation
{
    /// <summary>
    /// 物料报废ng信息接口
    /// </summary>
    public interface IWhMaterialInventoryScrapNgRelationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryScrapNgRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(IEnumerable<WhMaterialInventoryScrapNgRelationEntity> whMaterialInventoryScrapNgRelationEntity);

        /// <summary>
        /// 根据ID查询List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhMaterialInventoryScrapNgRelationEntity>> GetByIdsAsync(IEnumerable<long> materialInventoryScrapIds);
    }
}