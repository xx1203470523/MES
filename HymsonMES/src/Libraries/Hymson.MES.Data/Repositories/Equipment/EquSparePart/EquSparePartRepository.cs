using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart
{
    /// <summary>
    /// 备件注册仓储
    /// </summary>
    public partial class EquSparePartRepository : IEquSparePartRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparePartRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(EquSparePartEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, entity);
            entity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartEntity entity)
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
        public async Task<EquSparePartEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquSparePartEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparePartPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartEntity>> GetPagedInfoAsync(EquSparePartPagedQuery equSparePartPagedQuery)
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
           
            var offSet = (equSparePartPagedQuery.PageIndex - 1) * equSparePartPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSparePartPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSparePartPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equSparePartEntitiesTask = conn.QueryAsync<EquSparePartEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSparePartEntities = await equSparePartEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparePartEntity>(equSparePartEntities, equSparePartPagedQuery.PageIndex, equSparePartPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSparePartQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartEntity>> GetEquSparePartEntitiesAsync(EquSparePartQuery equSparePartQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equSparePartEntities = await conn.QueryAsync<EquSparePartEntity>(template.RawSql, equSparePartQuery);
            return equSparePartEntities;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSparePartRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_sparepart` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_sparepart` /**where**/";
        const string GetEquSparePartEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_sparepart`(  `Id`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `SiteCode`) VALUES (   @Id, @SparePartCode, @SparePartName, @SparePartTypeId, @ProcMaterialId, @UnitId, @IsKey, @IsStandard, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_sparepart` SET   SparePartCode = @SparePartCode, SparePartName = @SparePartName, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreateBy = @CreateBy, CreateOn = @CreateOn, UpdateBy = @UpdateBy, UpdateOn = @UpdateOn, IsDeleted = @IsDeleted, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_sparepart` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`, `SiteCode`
                            FROM `equ_sparepart`  WHERE Id = @Id ";
    }
}
