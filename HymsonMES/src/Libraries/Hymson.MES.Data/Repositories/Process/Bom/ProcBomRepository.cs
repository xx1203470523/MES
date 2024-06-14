using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM表仓储
    /// </summary>
    public partial class ProcBomRepository : BaseRepository, IProcBomRepository
    {
        public ProcBomRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBomEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            if (!ids.Any()) return new List<ProcBomEntity>();

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomEntity>> GetPagedInfoAsync(ProcBomPagedQuery procBomPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procBomPagedQuery.BomCode))
            {
                procBomPagedQuery.BomCode = $"%{procBomPagedQuery.BomCode}%";
                sqlBuilder.Where(" BomCode like @BomCode ");
            }
            if (!string.IsNullOrWhiteSpace(procBomPagedQuery.BomName))
            {
                procBomPagedQuery.BomName = $"%{procBomPagedQuery.BomName}%";
                sqlBuilder.Where(" BomName like @BomName ");
            }
            if (!string.IsNullOrWhiteSpace(procBomPagedQuery.Version))
            {
                procBomPagedQuery.Version = $"%{procBomPagedQuery.Version}%";
                sqlBuilder.Where(" Version like @Version ");
            }
            if (procBomPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status = @Status ");
            }

            var offSet = (procBomPagedQuery.PageIndex - 1) * procBomPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBomPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBomPagedQuery);

            using var conn = GetMESDbConnection();
            var procBomEntitiesTask = conn.QueryAsync<ProcBomEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBomEntities = await procBomEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBomEntity>(procBomEntities, procBomPagedQuery.PageIndex, procBomPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBomQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomEntity>> GetProcBomEntitiesAsync(ProcBomQuery procBomQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBomEntitiesSqlTemplate);
            sqlBuilder.Where(" IsDeleted = 0 ");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procBomQuery.BomCode))
            {
                sqlBuilder.Where(" BomCode = @BomCode ");
            }
            if (!string.IsNullOrWhiteSpace(procBomQuery.Version))
            {
                sqlBuilder.Where(" Version = @Version ");
            }

            using var conn = GetMESDbConnection();
            var procBomEntities = await conn.QueryAsync<ProcBomEntity>(template.RawSql, procBomQuery);
            return procBomEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBomEntity procBomEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBomEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBomEntity> procBomEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBomEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBomEntity procBomEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBomEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBomEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBomEntity> procBomEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procBomEntitys);
        }

        /// <summary>
        /// 更新 BOM IsCurrentVersion 为 false
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> UpdateIsCurrentVersionIsFalseAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateIsCurrentVersionIsFalseSql, new { ids = ids });
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 根据编码获取Bom信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomEntity>> GetByCodesAsync(ProcBomsByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomEntity>(GetByCodesSql, param);
        }
    }

    public partial class ProcBomRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bom` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bom` /**where**/ ";
        const string GetProcBomEntitiesSqlTemplate = @"SELECT  /**select**/ FROM `proc_bom` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bom`( `Id`, `SiteId`, `BomCode`, `BomName`, `Status`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @BomCode, @BomName, @Status, @Version, @IsCurrentVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_bom` SET BomName = @BomName, IsCurrentVersion = @IsCurrentVersion, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        /// <summary>
        /// 更新 BOM IsCurrentVersion 为 false
        /// </summary>
        const string UpdateIsCurrentVersionIsFalseSql = "UPDATE `proc_bom` SET IsCurrentVersion = false WHERE Id in @ids ";
        const string UpdatesSql = "UPDATE `proc_bom` SET  BomName = @BomName, Status = @Status, Version = @Version, IsCurrentVersion = @IsCurrentVersion, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_bom` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bom` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `proc_bom`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_bom`  WHERE Id IN @ids ";

        const string UpdateStatusSql = "UPDATE `proc_bom` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        const string GetByCodesSql = @"SELECT * FROM `proc_bom` WHERE BomCode IN @Codes AND SiteId= @SiteId  AND IsDeleted=0 ";
    }
}
