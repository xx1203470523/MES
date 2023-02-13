using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartType.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePartType
{
    /// <summary>
    /// 备件类型仓储
    /// </summary>
    public partial class EquSparePartTypeRepository : IEquSparePartTypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparePartTypeRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparePartTypeEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(EquSparePartTypeEntity equSparePartTypeEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, equSparePartTypeEntity);
            equSparePartTypeEntity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparePartTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartTypeEntity equSparePartTypeEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, equSparePartTypeEntity);
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
        public async Task<EquSparePartTypeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquSparePartTypeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparePartTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartTypeEntity>> GetPagedInfoAsync(EquSparePartTypePagedQuery equSparePartTypePagedQuery)
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
           
            var offSet = (equSparePartTypePagedQuery.PageIndex - 1) * equSparePartTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSparePartTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(equSparePartTypePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equSparePartTypeEntitiesTask = conn.QueryAsync<EquSparePartTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSparePartTypeEntities = await equSparePartTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparePartTypeEntity>(equSparePartTypeEntities, equSparePartTypePagedQuery.PageIndex, equSparePartTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSparePartTypeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartTypeEntity>> GetEquSparePartTypeEntitiesAsync(EquSparePartTypeQuery equSparePartTypeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartTypeEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equSparePartTypeEntities = await conn.QueryAsync<EquSparePartTypeEntity>(template.RawSql, equSparePartTypeQuery);
            return equSparePartTypeEntities;
        }

    }

    public partial class EquSparePartTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spare_part_type` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_spare_part_type` /**where**/";
        const string GetEquSparePartTypeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spare_part_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spare_part_type`(  `Id`, `SparePartTypeCode`, `SparePartTypeName`, `Status`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `SiteCode`) VALUES (   @Id, @SparePartTypeCode, @SparePartTypeName, @Status, @Remark, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_spare_part_type` SET   SparePartTypeCode = @SparePartTypeCode, SparePartTypeName = @SparePartTypeName, Status = @Status, Remark = @Remark, CreateBy = @CreateBy, CreateOn = @CreateOn, UpdateBy = @UpdateBy, UpdateOn = @UpdateOn, IsDeleted = @IsDeleted, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_spare_part_type` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SparePartTypeCode`, `SparePartTypeName`, `Status`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `SiteCode`
                            FROM `equ_spare_part_type`  WHERE Id = @Id ";
    }
}
