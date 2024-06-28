using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process.MaskCode
{
    /// <summary>
    /// 仓储（掩码维护）
    /// </summary>
    public partial class ProcMaskCodeRepository : BaseRepository, IProcMaskCodeRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcMaskCodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcMaskCodeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcMaskCodeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaskCodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaskCodeEntity>(GetByIdSql, new { IsDeleted = 0, id });
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcMaskCodeEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaskCodeEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaskCodeEntity>> GetPagedListAsync(ProcMaskCodePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*,code as maskCode");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<ProcMaskCodeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<ProcMaskCodeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaskCodeEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaskCodeEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 根据编码获取掩码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaskCodeEntity>> GetByCodesAsync(ProcMaskCodesByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaskCodeEntity>(GetByCodesSql, param);
        }
    }

    /// <summary>
    /// 仓储（掩码维护）
    /// </summary>
    public partial class ProcMaskCodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `proc_maskcode`(`Id`, `SiteId`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `proc_maskcode` SET Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_maskcode` SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string GetByIdSql = "SELECT `Id`, `SiteId`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn` FROM `proc_maskcode` WHERE `IsDeleted` = @IsDeleted AND `Id` = @id;";
        const string GetByCodeSql = "SELECT * FROM proc_maskcode WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `proc_maskcode` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_maskcode` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetByIdsSql = @"SELECT * FROM `proc_maskcode` WHERE Id IN @ids AND IsDeleted=0 ";

        const string GetByCodesSql = @"SELECT * FROM `proc_maskcode` WHERE Code IN @Codes AND SiteId= @SiteId  AND IsDeleted=0 ";
    }
}
