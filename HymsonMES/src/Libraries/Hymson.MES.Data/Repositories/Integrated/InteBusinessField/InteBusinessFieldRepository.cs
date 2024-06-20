using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteBusinessField.View;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（字段定义）
    /// </summary>
    public partial class InteBusinessFieldRepository : BaseRepository, IInteBusinessFieldRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteBusinessFieldRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteBusinessFieldEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteBusinessFieldEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteBusinessFieldEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteBusinessFieldEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteBusinessFieldEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteBusinessFieldEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldEntity>> GetEntitiesAsync(InteBusinessFieldQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteBusinessFieldView>> GetPagedListAsync(InteBusinessFieldPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("v.UpdatedOn DESC");
            sqlBuilder.Where("v.IsDeleted = 0");
            sqlBuilder.Where("v.SiteId = @SiteId");

            if (!string.IsNullOrEmpty(pageQuery.Code))
            {
                pageQuery.Code = $"%{pageQuery.Code}%";
                sqlBuilder.Where(" v.Code like @Code ");
            }
            if (!string.IsNullOrEmpty(pageQuery.Name))
            {
                pageQuery.Name = $"%{pageQuery.Name}%";
                sqlBuilder.Where(" v.Name like  @Name ");
            }

            if (pageQuery.CreatedOn != null && pageQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pageQuery.CreatedOn[0], CreatedOnEnd = pageQuery.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where(" v.CreatedOn >= @CreatedOnStart AND v.CreatedOn < @CreatedOnEnd ");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<InteBusinessFieldView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteBusinessFieldView>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteBusinessFieldEntity> GetByCodeAsync(InteBusinessFieldQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteBusinessFieldEntity>(GetByCodeSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteBusinessFieldRepository
    {
       // const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_business_field /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                        v.*,vt.Code AS MaskCode,vt.Name AS MaskName  
                     FROM `inte_business_field` v 
                     LEFT JOIN `proc_maskcode` vt ON vt.Id=v.MaskCodeId
                    /**where**/ ORDER BY v.UpdatedOn DESC LIMIT @Offset,@Rows ";
        //const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_business_field /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";

        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(*) 
                     FROM `inte_business_field` v 
                     LEFT JOIN `proc_maskcode` vt ON vt.Id=v.MaskCodeId 
                    /**where**/  ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_business_field /**where**/  ";

        const string InsertSql = "INSERT INTO inte_business_field(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `Source`, `MaskCodeId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @Type, @Source, @MaskCodeId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO inte_business_field(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `Source`, `MaskCodeId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @Type, @Source, @MaskCodeId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE inte_business_field SET   SiteId = @SiteId, Code = @Code, Name = @Name, Type = @Type, Source = @Source, MaskCodeId = @MaskCodeId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_business_field SET   SiteId = @SiteId, Code = @Code, Name = @Name, Type = @Type, Source = @Source, MaskCodeId = @MaskCodeId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_business_field SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_business_field SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_business_field WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_business_field WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT * 
                            FROM `inte_business_field`  WHERE Code = @Code AND IsDeleted=0 AND SiteId=@SiteId ";

    }
}
