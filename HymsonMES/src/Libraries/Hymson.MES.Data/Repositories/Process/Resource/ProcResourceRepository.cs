using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源维护表仓储层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcResourceRepository : IProcResourceRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcResourceRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        ///  查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceView>> GetPageListAsync(ProcResourcePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            //sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(query.SiteCode))
            {
                sqlBuilder.Where("a.SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }
            if (!string.IsNullOrWhiteSpace(query.ResType))
            {
                query.ResType = $"%{query.ResType}%";
                sqlBuilder.Where("ResType like @ResType");
            }
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceView>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEntity>> GetListAsync(ProcResourcePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(query.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(query.ResName))
            {
                query.ResName = $"%{query.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcResourceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceEntity>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="resourceTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新资源维护数据
        /// </summary>
        /// <param name="updateCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = idsArr });
        }
    }

    public partial class ProcResourceRepository
    {
        const string GetByIdSql = "select * from proc_resource where Id =@Id ";
        const string Get = "select `proc_resource WHERE `Id` in @Id and ResTypeId >0 ";

        const string GetPagedInfoDataSqlTemplate = "SELECT a.*,b.ResType,b.ResTypeName  FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.I /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT count(*) FROM proc_resource a left join proc_resource_type b on a.ResTypeId =b.Id  /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource`(`Id`, `SiteCode`, `ResCode`, `ResName`,`Status`,`ResTypeId, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @ResCode, @ResName,@Status,@ResTypeId,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `proc_resource` SET ResName = @ResName,ResTypeId = @ResTypeId,Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource` SET `IsDeleted` = 1 WHERE `Id` in @Id;";
    }
}
