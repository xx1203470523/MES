/*
 *creator: Karl
 *
 *describe: 资源设备绑定表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 11:20:47
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
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
            sqlBuilder.Where("a.IsDeleted=0");
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
        /// 根据资源id和设备Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetByResourceIdAsync(ProcResourceEquipmentBindQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetByResourceIdSqllTemplate);
            sqlBuilder.Where("IsDeleted=0");
            if (query.ResourceId > 0)
            {
                sqlBuilder.Where("ResourceId=@ResourceId");
            }
            if (query.IsMain)
            {
                sqlBuilder.Where("IsMain=@IsMain");
            }
            if (query.Ids.Length > 0)
            {
                sqlBuilder.Where("EquipmentId in @Ids");
            }
            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }
            sqlBuilder.AddParameters(query);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEquipmentBindView>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 批量根据资源Id查询绑定信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceEquipmentBindEntity>> GetByResourceIdsAsync(long[] ids)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetByResourceIdSqllTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("ResourceId in @ResourceIds");
            sqlBuilder.AddParameters(new { ResourceIds = ids });
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceEquipmentBindView>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bindEntitys"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<ProcResourceEquipmentBindEntity> bindEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            await conn.ExecuteAsync(InsertSql, bindEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceEquipmentBinds"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcResourceEquipmentBindEntity> procResourceEquipmentBinds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceEquipmentBinds);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesRangeAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Ids = idsArr });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByResourceIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByResourceIdSql, new { ResourceId = id });
        }
    }

    public partial class ProcResourceEquipmentBindRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.EquipmentCode,b.EquipmentName,b.EquipmentDesc from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/ ORDER BY a.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_equipment_bind a left join equ_equipment b on a.EquipmentId=b.Id and b.IsDeleted=0 /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_equipment_bind`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `IsMain`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResourceId, @EquipmentId, @IsMain, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_equipment_bind` SET  EquipmentId=@EquipmentId,IsMain=@IsMain,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_equipment_bind` SET IsDeleted = Id WHERE Id in @Ids ";
        const string DeleteByResourceIdSql = "delete from `proc_resource_equipment_bind` WHERE ResourceId = @ResourceId ";
        const string GetByResourceIdSqllTemplate = "SELECT * FROM proc_resource_equipment_bind /**where**/  ";
    }
}
