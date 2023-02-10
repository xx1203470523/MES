/*
 *creator: Karl
 *
 *describe: 资源设备绑定表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源设备绑定表仓储
    /// </summary>
    public partial class ProcResourceEquipmentBindRepository : IProcResourceEquipmentBindRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcResourceEquipmentBindRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEquipmentBindView>> GetPagedInfoAsync(ProcResourceEquipmentBindPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("a.ResourceId=@ResourceId");
            //TODO 按更新时间倒序排列

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEquipmentBindEntitiesTask = conn.QueryAsync<ProcResourceEquipmentBindView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEquipmentBindEntities = await procResourceEquipmentBindEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceEquipmentBindView>(procResourceEquipmentBindEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procResourceEquipmentBindQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetProcResourceEquipmentBindEntitiesAsync(ProcResourceEquipmentBindPagedQuery procResourceEquipmentBindQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcResourceEquipmentBindEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEquipmentBindEntities = await conn.QueryAsync<ProcResourceEquipmentBindEntity>(template.RawSql, procResourceEquipmentBindQuery);
            return procResourceEquipmentBindEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEquipmentBindEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEquipmentBindEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceEquipmentBindEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(ProcResourceEquipmentBindEntity procResourceEquipmentBindEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, procResourceEquipmentBindEntity);
            procResourceEquipmentBindEntity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceEquipmentBindEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceEquipmentBindEntity procResourceEquipmentBindEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceEquipmentBindEntity);
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
    }

    public partial class ProcResourceEquipmentBindRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.EquipmentCode,b.EquipmentName,b.EquipmentDesc from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/";
        const string GetProcResourceEquipmentBindEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_resource_equipment_bind` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_resource_equipment_bind`(  `Id`, `SiteCode`, `ResourceId`, `EquipmentId`, `IsMain`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @EquipmentId, @IsMain, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_equipment_bind` SET   SiteCode = @SiteCode, ResourceId = @ResourceId, EquipmentId = @EquipmentId, IsMain = @IsMain, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_equipment_bind` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ResourceId`, `EquipmentId`, `IsMain`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_resource_equipment_bind`  WHERE Id = @Id ";
    }
}
