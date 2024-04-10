/*
 *creator: Karl
 *
 *describe: 降级规则 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级规则仓储
    /// </summary>
    public partial class ManuDowngradingRuleRepository :BaseRepository, IManuDowngradingRuleRepository
    {

        public ManuDowngradingRuleRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuDowngradingRuleEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuDowngradingRuleEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingRuleEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuDowngradingRuleEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuDowngradingRulePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingRuleEntity>> GetPagedInfoAsync(ManuDowngradingRulePagedQuery manuDowngradingRulePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(manuDowngradingRulePagedQuery.Code))
            {
                manuDowngradingRulePagedQuery.Code = $"%{manuDowngradingRulePagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(manuDowngradingRulePagedQuery.Name))
            {
                manuDowngradingRulePagedQuery.Name = $"%{manuDowngradingRulePagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(manuDowngradingRulePagedQuery.OrCode))
            {
                manuDowngradingRulePagedQuery.OrCode = $"%{manuDowngradingRulePagedQuery.OrCode}%";
                sqlBuilder.OrWhere("Code LIKE @OrCode");
            }
            if (!string.IsNullOrWhiteSpace(manuDowngradingRulePagedQuery.OrName))
            {
                manuDowngradingRulePagedQuery.OrName = $"%{manuDowngradingRulePagedQuery.OrName}%";
                sqlBuilder.OrWhere("Name LIKE @OrName");
            }

            var offSet = (manuDowngradingRulePagedQuery.PageIndex - 1) * manuDowngradingRulePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuDowngradingRulePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuDowngradingRulePagedQuery);

            using var conn = GetMESDbConnection();
            var manuDowngradingRuleEntitiesTask = conn.QueryAsync<ManuDowngradingRuleEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuDowngradingRuleEntities = await manuDowngradingRuleEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuDowngradingRuleEntity>(manuDowngradingRuleEntities, manuDowngradingRulePagedQuery.PageIndex, manuDowngradingRulePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuDowngradingRuleQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingRuleEntity>> GetManuDowngradingRuleEntitiesAsync(ManuDowngradingRuleQuery manuDowngradingRuleQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuDowngradingRuleEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            var manuDowngradingRuleEntities = await conn.QueryAsync<ManuDowngradingRuleEntity>(template.RawSql, manuDowngradingRuleQuery);
            return manuDowngradingRuleEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingRuleEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuDowngradingRuleEntity manuDowngradingRuleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuDowngradingRuleEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuDowngradingRuleEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingRuleEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuDowngradingRuleEntity manuDowngradingRuleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuDowngradingRuleEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuDowngradingRuleEntitys);
        }
        #endregion

        /// <summary>
        /// 获取最大排序序号的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuDowngradingRuleEntity> GetMaxSerialNumberAsync(long siteId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuDowngradingRuleEntity>(GetMaxSerialNumberSql, new { SiteId = siteId });
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuDowngradingRuleEntity> GetByCodeAsync(ManuDowngradingRuleCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuDowngradingRuleEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 批量更新序号
        /// </summary>
        /// <param name="manuDowngradingRuleEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateSerialNumbersAsync(List<ManuDowngradingRuleEntity> manuDowngradingRuleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSerialNumbersSql, manuDowngradingRuleEntitys);
        }
    }

    public partial class ManuDowngradingRuleRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_downgrading_rule` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY SerialNumber LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_downgrading_rule` /**where**/ ";
        const string GetManuDowngradingRuleEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_downgrading_rule` /**where**/ 
                                            ORDER BY SerialNumber ";

        const string InsertSql = "INSERT INTO `manu_downgrading_rule`(  `Id`, `SiteId`, `SerialNumber`, `Code`, `Name`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNumber, @Code, @Name, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_downgrading_rule`(  `Id`, `SiteId`, `SerialNumber`, `Code`, `Name`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNumber, @Code, @Name, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_downgrading_rule` SET Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_downgrading_rule` SET   SiteId = @SiteId, SerialNumber = @SerialNumber, Code = @Code, Name = @Name, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_downgrading_rule` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_downgrading_rule` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SerialNumber`, `Code`, `Name`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_downgrading_rule`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SerialNumber`, `Code`, `Name`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_downgrading_rule`  WHERE Id IN @Ids ";
        #endregion

        const string GetMaxSerialNumberSql = @"SELECT * FROM `manu_downgrading_rule`  WHERE SiteId =@SiteId AND IsDeleted=0 ORDER BY SerialNumber DESC ";
        const string GetByCodeSql = @"SELECT * 
                            FROM `manu_downgrading_rule`  WHERE Code = @Code AND IsDeleted=0 AND SiteId=@SiteId ";
        const string UpdateSerialNumbersSql = @"UPDATE `manu_downgrading_rule` SET  SerialNumber = @SerialNumber, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
    }
}
