/*
 *creator: Karl
 *
 *describe: 二维载具条码明细 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-19 08:14:38
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 二维载具条码明细仓储
    /// </summary>
    public partial class InteVehiceFreightStackRepository :BaseRepository, IInteVehiceFreightStackRepository
    {

        public InteVehiceFreightStackRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehiceFreightStackEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehiceFreightStackEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehiceFreightStackEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehiceFreightStackEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehiceFreightStackPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehiceFreightStackEntity>> GetPagedInfoAsync(InteVehiceFreightStackPagedQuery inteVehiceFreightStackPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (inteVehiceFreightStackPagedQuery.PageIndex - 1) * inteVehiceFreightStackPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehiceFreightStackPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehiceFreightStackPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntitiesTask = conn.QueryAsync<InteVehiceFreightStackEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehiceFreightStackEntities = await inteVehiceFreightStackEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehiceFreightStackEntity>(inteVehiceFreightStackEntities, inteVehiceFreightStackPagedQuery.PageIndex, inteVehiceFreightStackPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehiceFreightStackQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehiceFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQuery inteVehiceFreightStackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehiceFreightStackEntitiesSqlTemplate);
            sqlBuilder.Where("SiteId = @SiteId");
            if(inteVehiceFreightStackQuery.VehicleId!=null)
            {
                sqlBuilder.Where("VehicleId = @VehicleId");
            }
            if (inteVehiceFreightStackQuery.LocationId != null)
            {
                sqlBuilder.Where("LocationId = @LocationId");
            }
            if (inteVehiceFreightStackQuery.Sfcs!=null&&inteVehiceFreightStackQuery.Sfcs.Any())
            {
                sqlBuilder.Where("BarCode IN @Sfcs");
            }
            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntities = await conn.QueryAsync<InteVehiceFreightStackEntity>(template.RawSql, inteVehiceFreightStackQuery);
            return inteVehiceFreightStackEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehiceFreightStackEntity inteVehiceFreightStackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehiceFreightStackEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehiceFreightStackEntity> inteVehiceFreightStackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehiceFreightStackEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehiceFreightStackEntity inteVehiceFreightStackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehiceFreightStackEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehiceFreightStackEntity> inteVehiceFreightStackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehiceFreightStackEntitys);
        }

        public async Task<IEnumerable<InteVehiceFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQueryByLocation inteVehiceFreightStackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehiceFreightStackEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntities = await conn.QueryAsync<InteVehiceFreightStackEntity>(template.RawSql, inteVehiceFreightStackQuery);
            return inteVehiceFreightStackEntities;
        }

        public async Task<InteVehiceFreightStackEntity> GetBySFCAsync(string sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehiceFreightStackEntity>(GetBySFCSql, new { BarCode = sfc });
        }
        #endregion

    }

    public partial class InteVehiceFreightStackRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehice_freight_stack` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehice_freight_stack` /**where**/ ";
        const string GetInteVehiceFreightStackEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehice_freight_stack` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehice_freight_stack`(  `Id`, `SiteId`, `LocationId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @LocationId, @BarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";
        const string InsertsSql = "INSERT INTO `inte_vehice_freight_stack`(  `Id`, `SiteId`, `LocationId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @LocationId, @BarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";

        const string UpdateSql = "UPDATE `inte_vehice_freight_stack` SET   SiteId = @SiteId, LocationId = @LocationId, BarCode = @BarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehice_freight_stack` SET   SiteId = @SiteId, LocationId = @LocationId, BarCode = @BarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "DELETE `inte_vehice_freight_stack`  WHERE Id = @Id ";
        const string DeletesSql = "DELETE `inte_vehice_freight_stack`  WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `LocationId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `inte_vehice_freight_stack`  WHERE Id = @Id ";
        const string GetBySFCSql = @"SELECT 
                               `Id`, `SiteId`, `LocationId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `inte_vehice_freight_stack`  WHERE BarCode = @BarCode ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `LocationId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `inte_vehice_freight_stack`  WHERE Id IN @Ids ";
        #endregion
    }
}
