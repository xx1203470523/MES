using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 生产班次仓储
    /// </summary>
    public partial class InteClassRepository : IInteClassRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteClassRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteClassEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteClassEntity inteClassEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, inteClassEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteClassEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteClassEntity inteClassEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteClassEntity);
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
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, idsArr);
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
        /// 分页查询
        /// </summary>
        /// <param name="inteClassPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteClassEntity>> GetPagedInfoAsync(InteClassPagedQuery inteClassPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            //sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

            var offSet = (inteClassPagedQuery.PageIndex - 1) * inteClassPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteClassPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteClassPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteClassEntitiesTask = conn.QueryAsync<InteClassEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteClassEntities = await inteClassEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteClassEntity>(inteClassEntities, inteClassPagedQuery.PageIndex, inteClassPagedQuery.PageSize, totalCount);
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
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_class` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_class` /**where**/";
        const string GetInteClassEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_class` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_class`(  `Id`, `ClassName`, `ClassType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @ClassName, @ClassType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `inte_class` SET   ClassName = @ClassName, ClassType = @ClassType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_class` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `ClassName`, `ClassType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`
                            FROM `inte_class`  WHERE Id = @Id ";
    }
}
