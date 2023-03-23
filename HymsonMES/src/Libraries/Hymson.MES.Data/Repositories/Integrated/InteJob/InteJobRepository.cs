using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated.InteJob
{
    /// <summary>
    /// 作业表仓储
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public partial class InteJobRepository : IInteJobRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteJobRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobEntity>> GetPagedInfoAsync(InteJobPagedQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            if (string.IsNullOrEmpty(param.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(param.Sorting);
            }
            sqlBuilder.Select("SiteId,Id,Code,Name,ClassProgram,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted");
            if (param.SiteId != null) { sqlBuilder.Where("SiteId = @SiteId"); }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                param.Code = $"%{param.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                param.Code = $"%{param.Name}%";
                sqlBuilder.Where("Name like @Name");
            }


            var offSet = (param.PageIndex - 1) * param.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = param.PageSize });
            sqlBuilder.AddParameters(param);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteJobEntitiesTask = conn.QueryAsync<InteJobEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteJobEntities = await inteJobEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteJobEntity>(inteJobEntities, param.PageIndex, param.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteJobEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteJobEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteJobEntity>(GetByIdsSql, new { ids = ids });
        }
        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InteJobEntity> GetByCodeAsync(EntityByCodeQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteJobEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteJobEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, param);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertRangAsync(IEnumerable<InteJobEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRangSql, param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteJobEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, param);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangAsync(IEnumerable<InteJobEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRangSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteRangSql, param);
        }
    }

    /// <summary>
    /// 作业表SQL语句
    /// @author admin
    /// @date 2023-02-21
    public partial class InteJobRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_job` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_job` /**where**/ ";
        const string InsertSql = "INSERT INTO  `inte_job` ( SiteId,Id,Code,Name,ClassProgram,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted) VALUES ( @SiteId,@Id,@Code,@Name,@ClassProgram,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted) ";
        const string UpdateSql = "UPDATE `inte_job` SET  Name=@Name,ClassProgram=@ClassProgram,Remark=@Remark,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,IsDeleted=@IsDeleted WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string UpdateRangSql = "UPDATE `inte_job` SET Name=@Name,ClassProgram=@ClassProgram,Remark=@Remark,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,IsDeleted=@IsDeleted WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string DeleteRangSql = "UPDATE `inte_job` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids AND IsDeleted=0";
        const string GetByIdSql = @"SELECT SiteId,Id,Code,Name,ClassProgram,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted FROM `inte_job`  WHERE Id = @Id AND IsDeleted=0  ";
        const string GetByIdsSql = @"SELECT  * FROM `inte_job`  WHERE Id IN @ids AND IsDeleted=0  ";
        const string GetByCodeSql = @"SELECT SiteId,Id,Code,Name,ClassProgram,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted FROM `inte_job`  WHERE Code = @Code  AND SiteId=@Site AND IsDeleted=0 ";
    }
}