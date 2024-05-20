using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using IdGen;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码追溯表）
    /// </summary>
    public partial class ManuSFCNodeRepository : BaseRepository, IManuSFCNodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSFCNodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            var sqlBuilder = new StringBuilder(InsertSql);
            foreach (var e in entities)
            {
                sqlBuilder.Append($"({e.Id}, {e.ProductId}, '{e.SFC}', '{e.Name}', '{e.Location}', @User, @Time, @User, @Time, {e.IsDeleted}, {e.SiteId}),");
            }

            // 移除最后一个逗号
            sqlBuilder.Length--;

            // 前面做了非空和数据量判断，所以这里直接取第一个元素
            var first = entities.First();

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sqlBuilder.ToString(), new { User = first.CreatedBy, Time = first.CreatedOn });

            /*
            // 使用StringBuilder来构建VALUES后面的括号集合
            var valuesBuilder = new StringBuilder();
            var parameters = new DynamicParameters();

            foreach (var e in entities)
            {
                // 使用参数化查询的占位符
                valuesBuilder.Append($"({e.Id}Id, @{e.Id}ProductId, @{e.Id}SFC, @{e.Id}Name, @{e.Id}Location, @{e.Id}CreatedBy, @{e.Id}CreatedOn, @{e.Id}UpdatedBy, @{e.Id}UpdatedOn, @{e.Id}IsDeleted, @{e.Id}SiteId),");

                // 构建每个对象的参数化值
                parameters.Add($"@{e.Id}Id", e.Id);
                parameters.Add($"@{e.Id}ProductId", e.ProductId);
                parameters.Add($"@{e.Id}SFC", e.SFC);
                parameters.Add($"@{e.Id}Name", e.Name);
                parameters.Add($"@{e.Id}Location", e.Location);
                parameters.Add($"@{e.Id}CreatedBy", e.CreatedBy);
                parameters.Add($"@{e.Id}CreatedOn", e.CreatedOn);
                parameters.Add($"@{e.Id}UpdatedBy", e.UpdatedBy);
                parameters.Add($"@{e.Id}UpdatedOn", e.UpdatedOn);
                parameters.Add($"@{e.Id}IsDeleted", e.IsDeleted);
                parameters.Add($"@{e.Id}SiteId", e.SiteId);
            }

            // 移除最后一个逗号
            valuesBuilder.Length--;

            // 将构建的VALUES集合添加到SQL语句中
            sqlBuilder.Append(valuesBuilder);

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sqlBuilder.ToString(), parameters);
            */
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> IgnoreRangeAsync(IEnumerable<ManuSFCNodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(IgnoreSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSFCNodeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSFCNodeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IEnumerable<long> ids)
        {
            if (ids == null || !ids.Any()) return 0;

            // 拼接删除SQL
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(DeleteSql);
            stringBuilder.Append('(');
            foreach (var id in ids)
            {
                stringBuilder.Append(id);

                // 如果不是最后一个元素，添加逗号
                if (id != ids.Last()) stringBuilder.Append(',');
            }
            stringBuilder.Append(')');

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(stringBuilder.ToString());
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSFCNodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSFCNodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSFCNodeEntity> GetBySFCAsync(EntityBySFCQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("SFC = @SFC");
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSFCNodeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeEntity>(GetByIdsSql, new { Ids = ids });
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSFCNodeRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_node /**where**/  ";

        const string InsertSql = "INSERT INTO manu_sfc_node (`Id`, `ProductId`, `SFC`, `Name`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES ";
        const string IgnoreSql = "INSERT IGNORE manu_sfc_node (`Id`, `ProductId`, `SFC`, `Name`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @ProductId, @SFC, @Name, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string DeleteSql = "DELETE FROM manu_sfc_node WHERE Id IN ";

        const string UpdateSql = "UPDATE manu_sfc_node SET   ProductId = @ProductId, SFC = @SFC, Name = @Name, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_sfc_node SET   ProductId = @ProductId, SFC = @SFC, Name = @Name, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_node WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_node WHERE Id IN @Ids ";

    }
}
