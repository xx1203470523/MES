/*
 *creator: Karl
 *
 *describe: BOM明细表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Utils;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细表仓储
    /// </summary>
    public partial class ProcBomDetailRepository : IProcBomDetailRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcBomDetailRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);

        }

        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteBomIDAsync(DeleteCommand command) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteBomIDsSql, new { command.UserId, command.DeleteOn, bomIds = command.Ids,command });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDetailEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcBomDetailEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcBomDetailEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 查询主物料表列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailView>> GetListMainAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

            return await conn.QueryAsync<ProcBomDetailView>(GetListMainSql, new { id = id });
        }

        /// <summary>
        /// 查询替代物料列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailView>> GetListReplaceAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

            return await conn.QueryAsync<ProcBomDetailView>(GetListReplaceSql, new { id = id });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDetailEntity>> GetPagedInfoAsync(ProcBomDetailPagedQuery procBomDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (procBomDetailPagedQuery.SiteId > 0)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }

            var offSet = (procBomDetailPagedQuery.PageIndex - 1) * procBomDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBomDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBomDetailPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procBomDetailEntitiesTask = conn.QueryAsync<ProcBomDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBomDetailEntities = await procBomDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBomDetailEntity>(procBomDetailEntities, procBomDetailPagedQuery.PageIndex, procBomDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBomDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetProcBomDetailEntitiesAsync(ProcBomDetailQuery procBomDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBomDetailEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procBomDetailEntities = await conn.QueryAsync<ProcBomDetailEntity>(template.RawSql, procBomDetailQuery);
            return procBomDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBomDetailEntity procBomDetailEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procBomDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBomDetailEntity> procBomDetailEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procBomDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBomDetailEntity procBomDetailEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procBomDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBomDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBomDetailEntity> procBomDetailEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procBomDetailEntitys);
        }

    }

    public partial class ProcBomDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bom_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_bom_detail` /**where**/ ";
        const string GetProcBomDetailEntitiesSqlTemplate = @"SELECT  /**select**/  FROM `proc_bom_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bom_detail`(`Id`, `SiteId`, `BomId`, `ProcedureId`, `MaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @BomId, @ProcedureId, @MaterialId, @ReferencePoint, @Usages, @Loss, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_bom_detail` SET  ProcedureId = @ProcedureId, MaterialId = @MaterialId, ReferencePoint = @ReferencePoint, Usages = @Usages, Loss = @Loss, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id,UpdatedBy = @UserId,UpdatedOn = @DeleteOn  WHERE Id in @ids";
        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        const string DeleteBomIDsSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id,UpdatedBy = @UserId,UpdatedOn = @DeleteOn WHERE BomId in @bomIds";
        const string GetByIdSql = @"SELECT * FROM `proc_bom_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_bom_detail`  WHERE Id IN @ids ";

        /// <summary>
        /// 查询主物料表列表
        /// </summary>
        const string GetListMainSql = @"SELECT 
                                          a.`Id`,a.MaterialId, 0 as BomDetailId,  a.`Usages`, a.`Loss`,a.`ReferencePoint`, a.`ProcedureId`, 
                     1 as IsMain, b.MaterialCode, b.MaterialName,
                     b.Version, c.Name, c.Code 
                            FROM `proc_bom_detail` a
                            INNER JOIN proc_material b on a.MaterialId = b.Id 
                            LEFT JOIN proc_procedure c on a.ProcedureId = c.Id
                            WHERE a.IsDeleted =0
                            AND a.BomId=@id
                            ORDER by a.UpdatedOn DESC ";
        /// <summary>
        /// 查询替代物料列表
        /// </summary>
        const string GetListReplaceSql = @"SELECT 
                                          a.`Id`,b.MaterialId, a.BomDetailId,
                                          a.`ReplaceMaterialId`, a.`Usages`, a.`Loss`,  a.`ReferencePoint`,b.ProcedureId, 0 as IsMain,
                     c.MaterialCode, c.MaterialName,c.Version, "" as Name, "" as Code
                            FROM `proc_bom_detail_replace_material` a
                            INNER JOIN proc_bom_detail b on a.BomDetailId = b.Id 
                            LEFT JOIN proc_material c on a.ReplaceMaterialId = c.Id
                            WHERE a.IsDeleted =0
                            AND a.BomId=@id
                            ORDER by a.UpdatedOn DESC ";
    }
}
