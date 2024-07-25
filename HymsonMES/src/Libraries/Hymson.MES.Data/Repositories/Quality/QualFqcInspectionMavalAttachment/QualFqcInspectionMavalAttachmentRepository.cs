/*
 *creator: Karl
 *
 *describe: 马威QFC检验附件 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 10:03:33
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualFqcInspectionMavalAttachment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMavalAttachment
{
    /// <summary>
    /// 马威QFC检验附件仓储
    /// </summary>
    public partial class QualFqcInspectionMavalAttachmentRepository : BaseRepository, IQualFqcInspectionMavalAttachmentRepository
    {

        public QualFqcInspectionMavalAttachmentRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
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
        /// <param name="ids"></param>
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
        public async Task<QualFqcInspectionMavalAttachmentEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcInspectionMavalAttachmentEntity>(GetByIdSql, new { Id = id });
        }


        /// <summary>
        /// 根据FqcMavalId获取数据
        /// </summary>
        /// <param name="fqcMavalId"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetByFqcMavalIdListAsync(long fqcMavalId)
        { 
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcInspectionMavalAttachmentEntity>(GetByFqcMavalIdSql, new { fqcMavalId });

        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcInspectionMavalAttachmentEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcInspectionMavalAttachmentEntity>> GetPagedInfoAsync(QualFqcInspectionMavalAttachmentPagedQuery qualFqcInspectionMavalAttachmentPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

            var offSet = (qualFqcInspectionMavalAttachmentPagedQuery.PageIndex - 1) * qualFqcInspectionMavalAttachmentPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = qualFqcInspectionMavalAttachmentPagedQuery.PageSize });
            sqlBuilder.AddParameters(qualFqcInspectionMavalAttachmentPagedQuery);

            using var conn = GetMESDbConnection();
            var qualFqcInspectionMavalAttachmentEntitiesTask = conn.QueryAsync<QualFqcInspectionMavalAttachmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualFqcInspectionMavalAttachmentEntities = await qualFqcInspectionMavalAttachmentEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcInspectionMavalAttachmentEntity>(qualFqcInspectionMavalAttachmentEntities, qualFqcInspectionMavalAttachmentPagedQuery.PageIndex, qualFqcInspectionMavalAttachmentPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetQualFqcInspectionMavalAttachmentEntitiesAsync(QualFqcInspectionMavalAttachmentQuery qualFqcInspectionMavalAttachmentQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetQualFqcInspectionMavalAttachmentEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var qualFqcInspectionMavalAttachmentEntities = await conn.QueryAsync<QualFqcInspectionMavalAttachmentEntity>(template.RawSql, qualFqcInspectionMavalAttachmentQuery);
            return qualFqcInspectionMavalAttachmentEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcInspectionMavalAttachmentEntity qualFqcInspectionMavalAttachmentEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, qualFqcInspectionMavalAttachmentEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualFqcInspectionMavalAttachmentEntity> qualFqcInspectionMavalAttachmentEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, qualFqcInspectionMavalAttachmentEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcInspectionMavalAttachmentEntity qualFqcInspectionMavalAttachmentEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, qualFqcInspectionMavalAttachmentEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualFqcInspectionMavalAttachmentEntity> qualFqcInspectionMavalAttachmentEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, qualFqcInspectionMavalAttachmentEntitys);
        }
        #endregion

    }

    public partial class QualFqcInspectionMavalAttachmentRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_fqc_inspection_maval_attachment` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_fqc_inspection_maval_attachment` /**where**/ ";
        const string GetQualFqcInspectionMavalAttachmentEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_fqc_inspection_maval_attachment` /**where**/  ";

        const string InsertSql = "INSERT INTO `qual_fqc_inspection_maval_attachment`(  `Id`, `FqcMavalId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @FqcMavalId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `qual_fqc_inspection_maval_attachment`(  `Id`, `FqcMavalId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @FqcMavalId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `qual_fqc_inspection_maval_attachment` SET   FqcMavalId = @FqcMavalId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_fqc_inspection_maval_attachment` SET   FqcMavalId = @FqcMavalId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `qual_fqc_inspection_maval_attachment` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_fqc_inspection_maval_attachment` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `FqcMavalId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `qual_fqc_inspection_maval_attachment`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `FqcMavalId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `qual_fqc_inspection_maval_attachment`  WHERE Id IN @Ids ";


        const string GetByFqcMavalIdSql = @"SELECT 
                               `Id`, `FqcMavalId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `qual_fqc_inspection_maval_attachment`  WHERE FqcMavalId = @fqcMavalId ";
        #endregion
    }
}
