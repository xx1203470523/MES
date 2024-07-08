/*
 *creator: Karl
 *
 *describe: 设备维修附件 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-01 04:52:14
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.EquRepairOrderAttachment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquRepairOrderAttachment
{
    /// <summary>
    /// 设备维修附件仓储
    /// </summary>
    public partial class EquRepairOrderAttachmentRepository :BaseRepository, IEquRepairOrderAttachmentRepository
    {

        public EquRepairOrderAttachmentRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<EquRepairOrderAttachmentEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairOrderAttachmentEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderAttachmentEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderAttachmentPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderAttachmentEntity>> GetPagedInfoAsync(EquRepairOrderAttachmentPagedQuery equRepairOrderAttachmentPagedQuery)
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
           
            var offSet = (equRepairOrderAttachmentPagedQuery.PageIndex - 1) * equRepairOrderAttachmentPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equRepairOrderAttachmentPagedQuery.PageSize });
            sqlBuilder.AddParameters(equRepairOrderAttachmentPagedQuery);

            using var conn = GetMESDbConnection();
            var equRepairOrderAttachmentEntitiesTask = conn.QueryAsync<EquRepairOrderAttachmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equRepairOrderAttachmentEntities = await equRepairOrderAttachmentEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquRepairOrderAttachmentEntity>(equRepairOrderAttachmentEntities, equRepairOrderAttachmentPagedQuery.PageIndex, equRepairOrderAttachmentPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equRepairOrderAttachmentQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetEquRepairOrderAttachmentEntitiesAsync(EquRepairOrderAttachmentQuery equRepairOrderAttachmentQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquRepairOrderAttachmentEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equRepairOrderAttachmentEntities = await conn.QueryAsync<EquRepairOrderAttachmentEntity>(template.RawSql, equRepairOrderAttachmentQuery);
            return equRepairOrderAttachmentEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquRepairOrderAttachmentEntity equRepairOrderAttachmentEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equRepairOrderAttachmentEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquRepairOrderAttachmentEntity> equRepairOrderAttachmentEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equRepairOrderAttachmentEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquRepairOrderAttachmentEntity equRepairOrderAttachmentEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equRepairOrderAttachmentEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquRepairOrderAttachmentEntity> equRepairOrderAttachmentEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equRepairOrderAttachmentEntitys);
        }
        #endregion

        /// <summary>
        /// 根据单据Id获取
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetByOrderIdAsync(EquRepairOrderAttachmentQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderAttachmentEntity>(GetByOrderIdSql, query);
        }

    }

    public partial class EquRepairOrderAttachmentRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_repair_order_attachment` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_repair_order_attachment` /**where**/ ";
        const string GetEquRepairOrderAttachmentEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_repair_order_attachment` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_repair_order_attachment`(  `Id`, `AttachmentType`, `RepairOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @AttachmentType, @RepairOrderId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_repair_order_attachment`(  `Id`, `AttachmentType`, `RepairOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @AttachmentType, @RepairOrderId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `equ_repair_order_attachment` SET   AttachmentType = @AttachmentType, RepairOrderId = @RepairOrderId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_repair_order_attachment` SET   AttachmentType = @AttachmentType, RepairOrderId = @RepairOrderId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_repair_order_attachment` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_repair_order_attachment` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `AttachmentType`, `RepairOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_repair_order_attachment`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `AttachmentType`, `RepairOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_repair_order_attachment`  WHERE Id IN @Ids ";
        const string GetByOrderIdSql = @"SELECT * FROM equ_repair_order_attachment WHERE IsDeleted = 0 AND AttachmentType=@AttachmentType AND   RepairOrderId = @OrderId ";
        #endregion
    }
}
