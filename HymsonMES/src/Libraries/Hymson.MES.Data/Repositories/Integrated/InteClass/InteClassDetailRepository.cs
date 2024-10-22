using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass
{
    /// <summary>
    /// 班制维护明细仓储
    /// </summary>
    public partial class InteClassDetailRepository : BaseRepository, IInteClassDetailRepository
    {
        public InteClassDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteClassDetailEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(InteClassDetailEntity inteClassDetailEntity)
        {
            using var conn = GetMESDbConnection();
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, inteClassDetailEntity);
            inteClassDetailEntity.Id = id;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteClassDetailEntity> entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteClassDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteClassDetailEntity inteClassDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteClassDetailEntity);
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
        /// 删除
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<int> DeleteByClassIdAsync(long classId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByClassId, new { classId });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteClassDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteClassDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteClassDetailEntity>> GetListByClassIdAsync(long classId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteClassDetailEntity>(GetListByClassId, new { classId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteClassDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteClassDetailEntity>> GetPagedListAsync(InteClassDetailPagedQuery inteClassDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");

            var offSet = (inteClassDetailPagedQuery.PageIndex - 1) * inteClassDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteClassDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteClassDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<InteClassDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<InteClassDetailEntity>(entities, inteClassDetailPagedQuery.PageIndex, inteClassDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteClassDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteClassDetailEntity>> GetInteClassDetailEntitiesAsync(InteClassDetailQuery inteClassDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteClassDetailEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteClassDetailEntities = await conn.QueryAsync<InteClassDetailEntity>(template.RawSql, inteClassDetailQuery);
            return inteClassDetailEntities;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteClassDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_class_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_class_detail` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetInteClassDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_class_detail` /**innerjoin**/ /**leftjoin**/ /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_class_detail`(  `Id`, `ClassId`, `DetailClassType`, `ProjectContent`, `StartTime`, `EndTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (   @Id, @ClassId, @DetailClassType, @ProjectContent, @StartTime, @EndTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId )  ";
        const string UpdateSql = "UPDATE `inte_class_detail` SET   ClassId = @ClassId, DetailClassType = @DetailClassType, ProjectContent = @ProjectContent, StartTime = @StartTime, EndTime = @EndTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_class_detail` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeleteByClassId = "DELETE FROM `inte_class_detail` WHERE ClassId = @classId ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `ClassId`, `DetailClassType`, `ProjectContent`, `StartTime`, `EndTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`
                            FROM `inte_class_detail`  WHERE Id = @Id ";
        const string GetListByClassId = @"SELECT 
                               `Id`, `ClassId`, `DetailClassType`, `ProjectContent`, `StartTime`, `EndTime`
                            FROM `inte_class_detail`  WHERE ClassId = @ClassId ";
    }
}
