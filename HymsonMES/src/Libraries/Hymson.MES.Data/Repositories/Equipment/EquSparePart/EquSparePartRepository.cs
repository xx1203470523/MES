using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Microsoft.Extensions.Options;
namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart
{
    /// <summary>
    /// 仓储（备件注册）
    /// </summary>
    public partial class EquSparePartRepository : BaseRepository, IEquSparePartRepository
    {
        /// <summary>
        /// 构造函数（备件注册）
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparePartRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparePartEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量修改备件的备件类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateSparePartTypeIdAsync(UpdateSparePartTypeIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSparePartTypeIdSql, command);
        }

        /// <summary>
        /// 清空备件的指定备件类型
        /// </summary>
        /// <param name="sparePartTypeId"></param>
        /// <returns></returns>
        public async Task<int> ClearSparePartTypeIdAsync(long sparePartTypeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ClearSparePartTypeIdSql, new { sparePartTypeId });
        }

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
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparePartEntity>(GetByIdSql, new { Id = id });
        }



        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartEntity>(GetByIdsSql, new { Ids = ids }); 
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartEntity>> GetPagedInfoAsync(EquSparePartPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("Type = @Type");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<EquSparePartEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquSparePartEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartEntity>> GetEntitiesAsync(EquSparePartQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equSparePartEntities = await conn.QueryAsync<EquSparePartEntity>(template.RawSql, query);
            return equSparePartEntities;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSparePartRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_sparepart` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_sparepart` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetEquSparePartEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart` /**innerjoin**/ /**leftjoin**/ /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_sparepart`(  `Id`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`) VALUES (   @Id, @SparePartCode, @SparePartName, @SparePartTypeId, @ProcMaterialId, @UnitId, @IsKey, @IsStandard, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_sparepart` SET SparePartCode = @SparePartCode, SparePartName = @SparePartName, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string UpdateSparePartTypeIdSql = "UPDATE `equ_sparepart` SET SparePartTypeId = @SparePartTypeId WHERE Id = @SparePartIds ";
        const string ClearSparePartTypeIdSql = "UPDATE `equ_sparepart` SET SparePartTypeId = 0 WHERE SparePartTypeId = @SparePartTypeId ";
        const string DeleteSql = "UPDATE `equ_sparepart` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`
                            FROM `equ_sparepart`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT *  FROM `equ_sparepart`  WHERE Id IN @Ids "; 
    }
}
