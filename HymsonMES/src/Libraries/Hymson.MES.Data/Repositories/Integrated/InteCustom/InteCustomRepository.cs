using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 客户维护仓储
    /// </summary>
    public partial class InteCustomRepository : BaseRepository, IInteCustomRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteCustomRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

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
        /// 根据Code获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<InteCustomEntity> GetByCodeAsync(string code)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCustomEntity>(GetByCodeSql, new { Code = code });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCustomEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCustomEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCustomPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCustomEntity>> GetPagedInfoAsync(InteCustomPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var inteCustomEntitiesTask = conn.QueryAsync<InteCustomEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteCustomEntities = await inteCustomEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCustomEntity>(inteCustomEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteCustomQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomEntity>> GetInteCustomEntitiesAsync(InteCustomQuery inteCustomQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCustomEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (inteCustomQuery.Codes != null && inteCustomQuery.Codes.Any())
            {
                sqlBuilder.Where(" Code IN @Codes ");
            }

            if (!string.IsNullOrWhiteSpace(inteCustomQuery.Code))
            {
                inteCustomQuery.Code = $"%{inteCustomQuery.Code}%";
                sqlBuilder.Where(" Code LIKE @Code ");
            }
            if (!string.IsNullOrWhiteSpace(inteCustomQuery.Name))
            {
                inteCustomQuery.Name = $"%{inteCustomQuery.Name}%";
                sqlBuilder.Where(" Name LIKE @Name ");
            }

            using var conn = GetMESDbConnection();
            var inteCustomEntities = await conn.QueryAsync<InteCustomEntity>(template.RawSql, inteCustomQuery);
            return inteCustomEntities;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomEntity>> GetEntitiesAsync(InteCustomQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCustomEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.Codes != null && query.Codes.Any())
            {
                sqlBuilder.Where(" Code IN @Codes ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCustomEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCustomEntity inteCustomEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteCustomEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCustomEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteCustomEntity> inteCustomEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteCustomEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCustomEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCustomEntity inteCustomEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteCustomEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteCustomEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteCustomEntity> inteCustomEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteCustomEntitys);
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteCustomRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_custom` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_custom` /**where**/ ";
        const string GetInteCustomEntitiesSqlTemplate = @"SELECT /**select**/ FROM `inte_custom` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_custom`(  `Id`, `Code`, `Name`, `Describe`, `Address`, `Telephone`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Describe, @Address, @Telephone, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `inte_custom`(  `Id`, `Code`, `Name`, `Describe`, `Address`, `Telephone`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Describe, @Address, @Telephone, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `inte_custom` SET  Name = @Name, `Describe` = @Describe, Address = @Address, Telephone = @Telephone, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_custom` SET  Name = @Name, `Describe` = @Describe, Address = @Address, Telephone = @Telephone, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_custom` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_custom` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Describe`, `Address`, `Telephone`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_custom`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Describe`, `Address`, `Telephone`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_custom`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT * 
                            FROM `inte_custom`  WHERE Code = @Code AND IsDeleted=0 ";
        #endregion
    }
}
