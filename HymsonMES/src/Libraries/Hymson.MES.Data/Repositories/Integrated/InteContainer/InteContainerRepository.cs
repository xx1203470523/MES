using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated.InteContainer
{
    /// <summary>
    /// 仓储（容器维护）
    /// </summary>
    public partial class InteContainerRepository : IInteContainerRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteContainerRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteContainerEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteContainerEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteContainerEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteContainerEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 通过关联ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteContainerEntity> GetByRelationIdAsync(InteContainerQuery query)
        {
            var sql = GetByMaterialIdSql;
            if (query.DefinitionMethod == DefinitionMethodEnum.MaterialGroup) sql = GetByMaterialGroupIdSql;

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteContainerEntity>(sql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteContainerView>> GetPagedInfoAsync(InteContainerPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IC.IsDeleted = 0");
            sqlBuilder.Where("IC.SiteId = @SiteId");
            sqlBuilder.OrderBy("IC.UpdatedOn DESC");

            sqlBuilder.Select("IC.Id, IC.Remark, IC.Status, IC.DefinitionMethod, IC.Level, IC.Maximum, IC.Minimum, IC.UpdatedBy, IC.UpdatedOn");
            sqlBuilder.Select("(CASE IC.DefinitionMethod WHEN 1 THEN M.MaterialName WHEN 2 THEN MG.GroupName ELSE '' END) AS Name, M.Version");
            sqlBuilder.LeftJoin("proc_material M ON IC.MaterialId = M.Id");
            sqlBuilder.LeftJoin("proc_material_group MG ON IC.MaterialGroupId = MG.Id");

            if (pagedQuery.DefinitionMethod.HasValue)
            {
                sqlBuilder.Where("IC.DefinitionMethod = @DefinitionMethod");
            }

            if (pagedQuery.Level.HasValue)
            {
                sqlBuilder.Where("IC.Level = @Level");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("IC.Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("(M.MaterialName LIKE @Name OR MG.GroupName LIKE @Name)");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Version))
            {
                pagedQuery.Version = $"%{pagedQuery.Version}%";
                sqlBuilder.Where("M.Version LIKE @Version");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteContainerView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<InteContainerView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteContainerRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_container IC /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_container IC /**innerjoin**/ /**leftjoin**/ /**where**/";

        const string InsertSql = "INSERT INTO `inte_container`( `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, Level, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, SiteId) VALUES (   @Id, @DefinitionMethod, @MaterialId, @MaterialGroupId, @Level, @Status, @Maximum, @Minimum, @Height, @Length, @Width, @MaxFillWeight, @Weight, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId)  ";
        const string UpdateSql = "UPDATE `inte_container` SET DefinitionMethod = @DefinitionMethod, MaterialId = @MaterialId, MaterialGroupId = @MaterialGroupId, Level = @Level, Status = @Status, Maximum = @Maximum, Minimum = @Minimum, Height = @Height, Length = @Length, Width = @Width, MaxFillWeight = @MaxFillWeight, Weight = @Weight, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE inte_container SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string GetByIdSql = @"SELECT 
                               `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, Level, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_container`  WHERE Id = @Id ";
        const string GetByMaterialIdSql = @"SELECT * FROM inte_container WHERE IsDeleted = 0 AND DefinitionMethod = @DefinitionMethod AND MaterialId = @MaterialId ";
        const string GetByMaterialGroupIdSql = @"SELECT * FROM inte_container WHERE IsDeleted = 0 AND DefinitionMethod = @DefinitionMethod AND MaterialGroupId = @MaterialGroupId ";
    }
}
