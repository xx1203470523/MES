using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
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
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteContainerEntity>> GetPagedInfoAsync(InteContainerPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Select("*");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteContainerEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<InteContainerEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteContainerRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_container` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_container` /**where**/";
        const string GetInteContainerEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_container` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_container`(  `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @DefinitionMethod, @MaterialId, @MaterialGroupId, @Status, @Maximum, @Minimum, @Height, @Length, @Width, @MaxFillWeight, @Weight, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `inte_container` SET   DefinitionMethod = @DefinitionMethod, MaterialId = @MaterialId, MaterialGroupId = @MaterialGroupId, Status = @Status, Maximum = @Maximum, Minimum = @Minimum, Height = @Height, Length = @Length, Width = @Width, MaxFillWeight = @MaxFillWeight, Weight = @Weight, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE inte_container SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE `Id` = @Ids;";
        const string GetByIdSql = @"SELECT 
                               `Id`, `DefinitionMethod`, `MaterialId`, `MaterialGroupId`, `Status`, `Maximum`, `Minimum`, `Height`, `Length`, `Width`, `MaxFillWeight`, `Weight`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_container`  WHERE Id = @Id ";
    }
}
