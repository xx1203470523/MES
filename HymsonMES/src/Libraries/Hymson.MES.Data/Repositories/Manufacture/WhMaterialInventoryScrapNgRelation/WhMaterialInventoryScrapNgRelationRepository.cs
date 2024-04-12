using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrapNgRelation
{
    /// <summary>
    /// 物料报废ng信息仓储
    /// </summary>
    public partial class WhMaterialInventoryScrapNgRelationRepository : BaseRepository, IWhMaterialInventoryScrapNgRelationRepository
    {
        public WhMaterialInventoryScrapNgRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryScrapNgRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(IEnumerable<WhMaterialInventoryScrapNgRelationEntity> whMaterialInventoryScrapNgRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, whMaterialInventoryScrapNgRelationEntity);
        }
        #endregion
        /// <summary>
        /// 根据ID查询List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryScrapNgRelationEntity>> GetByIdsAsync(IEnumerable<long> materialInventoryScrapIds)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfo1EntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where(" IsDeleted=0");
            sqlBuilder.Where(" MaterialInventoryScrapId in @materialInventoryScrapIds");
            using var conn = GetMESDbConnection();
            var manuSfcInfo1Entities = await conn.QueryAsync<WhMaterialInventoryScrapNgRelationEntity>(template.RawSql, new { materialInventoryScrapIds });
            return manuSfcInfo1Entities;
        }
    }

    public partial class WhMaterialInventoryScrapNgRelationRepository
    {
        #region 
        const string InsertSql = "INSERT INTO `wh_material_inventory_scrap_ng_relation`(  `Id`, `SiteId`, `MaterialInventoryScrapId`, `UnqualifiedCodeId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @MaterialInventoryScrapId, @UnqualifiedCodeId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string GetManuSfcInfo1EntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `wh_material_inventory_scrap_ng_relation` /**where**/  ";
        #endregion
    }
}