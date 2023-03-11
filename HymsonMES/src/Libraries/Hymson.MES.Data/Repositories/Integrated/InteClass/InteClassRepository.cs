using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 班制维护仓储
    /// </summary>
    public partial class InteClassRepository : IInteClassRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteClassRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteClassEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteClassEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
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
        public async Task<InteClassEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteClassEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteClassEntity>> GetPagedListAsync(InteClassPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.ClassName) == false)
            {
                pagedQuery.ClassName = $"%{pagedQuery.ClassName}%";
                sqlBuilder.Where("ClassName LIKE @ClassName");
            }

            if (pagedQuery.ClassType > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("ClassType = @ClassType");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteClassEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<InteClassEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteClassQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteClassEntity>> GetInteClassEntitiesAsync(InteClassQuery inteClassQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteClassEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteClassEntities = await conn.QueryAsync<InteClassEntity>(template.RawSql, inteClassQuery);
            return inteClassEntities;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteClassRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_class` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_class` /**where**/";
        const string GetInteClassEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_class` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_class`(  `Id`, `ClassName`, `ClassType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (   @Id, @ClassName, @ClassType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId )  ";
        const string UpdateSql = "UPDATE `inte_class` SET   ClassName = @ClassName, ClassType = @ClassType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_class` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `ClassName`, `ClassType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`
                            FROM `inte_class`  WHERE Id = @Id ";
    }
}
