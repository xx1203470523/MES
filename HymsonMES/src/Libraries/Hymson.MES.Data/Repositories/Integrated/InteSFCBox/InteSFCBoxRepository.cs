using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteSFCBox
{
    public partial class InteSFCBoxRepository : BaseRepository, IInteSFCBoxRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        public InteSFCBoxRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteSFCBoxEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<InteSFCBoxEntity> inteSFCBoxEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteSFCBoxEntitys);
        }

        /// <summary>
        /// 批量新增工单关联表
        /// </summary>
        /// <param name="inteSFCBoxWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertSFCBoxWorkOrderAsync(IEnumerable<InteSFCBoxWorkOrderEntity> inteSFCBoxWorkOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsWorkOrderSql, inteSFCBoxWorkOrderEntitys);
        }

        public async Task<PagedInfo<InteSFCBoxEntity>> GetPagedInfoAsync(InteSFCBoxQueryRep pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");


            if (!string.IsNullOrWhiteSpace(pagedQuery.BoxCode))
            {
                sqlBuilder.Where("BoxCode = @BoxCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.SFC))
            {
                sqlBuilder.Where("SFC = @SFC");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Grade))
            {
                sqlBuilder.Where("Grade = @Grade");
            }
            if (pagedQuery.Status != null && pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteSFCBoxEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<InteSFCBoxEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        public async Task<PagedInfo<InteSFCBoxBatch>> GetBoxCodeAsync(InteSFCBoxQueryRep pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetBatchNoSql);
            var templateCount = sqlBuilder.AddTemplate(GetBoxCodeCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");
            //sqlBuilder.OrderBy("msb.CreateOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.BatchNo))
            {
                sqlBuilder.Where("BatchNo = @BatchNo");
            }


            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteSFCBoxBatch>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<InteSFCBoxBatch>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 取同批次的所有箱码
        /// </summary>
        /// <param name="batchNos"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSFCAllView>> GetByBoxCodesAsync(string[] batchNos)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteSFCAllView>(GetByBoxCodesSql, new { BatchNos = batchNos });
        }

        /// <summary>
        /// 获取批次码相关信息
        /// </summary>
        /// <param name="boxCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSFCBoxEntity>> GetManuSFCBoxAsync(InteSFCBoxEntityQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetManuSFCBoxSql);
            sqlBuilder.Where("IsDeleted = 0");
            if (!string.IsNullOrWhiteSpace(query.BoxCode))
            {
                sqlBuilder.Where(" BoxCode = @BoxCode");
            }

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where(" SFC = @SFC");
            }

            if (query.SFCs != null && query.SFCs.Any())
            {
                sqlBuilder.Where(" SFC in @SFCs");
            }

            if (query.BoxCodes != null && query.BoxCodes.Any())
            {
                sqlBuilder.Where(" BoxCode in @BoxCodes");
            }

            if (!string.IsNullOrWhiteSpace(query.NotInBatch))
            {
                sqlBuilder.Where(" BatchNo != @NotInBatch");
            }


            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteSFCBoxEntity>(templateData.RawSql, templateData.Parameters);
       
        }

        /// <summary>
        /// 根据工单Id查询批次码
        /// </summary>
        /// <param name="boxCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderSFCBoxQuery>> GetByWorkOrderAsync(long workOrderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderSFCBoxQuery>(GetWorkOrderIdSql, new { WorkOrderId = workOrderId });
        }


        /// <summary>
        /// 删除工单与批次码关联
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSFCBoxWorkOrderAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteWorkOrderSql, new { WorkOrderId = id });
        }


        /// <summary>
        /// 按ID获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSFCBoxEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteSFCBoxEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }

    }



    public partial class InteSFCBoxRepository
    {
        const string InsertsSql = "INSERT INTO `manu_sfc_box`(  `Id`, `SiteId`, `BatchNo`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness`) VALUES (   @Id, @SiteId, @BatchNo,@SFC, @BoxCode, @Grade, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy,@UpdatedOn, @IsDeleted, @OCVB,@OCVBDate, @Weight, @DC, @DCDate, @IMPB, @SelfDischargeRate, @Width, @HeightZ, @HeightF,@ShoulderHeightZ, @ShoulderHeightF, @Thickness)  ";
        const string InsertsWorkOrderSql = "INSERT INTO `manu_sfc_box_workorder`( `Id`,`SiteId`,`BatchNo`,`BoxCode`,`WorkOrderId`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (   @Id,@SiteId,@BatchNo,@BoxCode,@WorkOrderId,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted)  ";
        const string DeleteWorkOrderSql = "delete FROM manu_sfc_box_workorder where WorkOrderId=@WorkOrderId";

        const string GetPagedInfoDataSqlTemplate = @"SELECT `Id`, `SiteId`,`BatchNo`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness` FROM manu_sfc_box /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";

        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc_box /**where**/";

        const string GetBoxCodeSqlTemplate = @"SELECT `Id`, `SiteId`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness` FROM manu_sfc_box WHERE id in(
            SELECT MIN(id) FROM manu_sfc_box msb  /**where**/  GROUP BY BatchNo HAVING COUNT(1)>1
            ) LIMIT @Offset,@Rows ";

        const string GetBatchNoSql = @"SELECT DISTINCT BatchNo, MAX(CreatedOn) AS CreatedOn FROM manu_sfc_box /**where**/ GROUP BY BatchNo ORDER BY CreatedOn DESC";
        const string GetBoxCodeCountSqlTemplate = @"SELECT COUNT(1) FROM (SELECT DISTINCT BatchNo FROM manu_sfc_box msb  /**where**/ ) as subquery";

        const string GetByBoxCodesSql = @"SELECT ROUND(max(OCVB),3)*1000-ROUND(min(OCVB),3)*1000 as OCVBDiff,round(max(IMPB),2) as MaxIMPB,BatchNo FROM `manu_sfc_box`  WHERE IsDeleted = 0 AND BatchNo IN @BatchNos group  by BatchNo";

        const string GetWorkOrderIdSql = @"select  * from manu_sfc_box_workorder where WorkOrderId =@WorkOrderId";

        const string GetByIdsSql = @"SELECT * FROM `manu_sfc_box`  WHERE Id IN @Ids ";

        const string DeleteSql = "UPDATE manu_sfc_box SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";

        const string GetManuSFCBoxSql = @"SELECT * FROM `manu_sfc_box`  /**where**/ ";
    }

}
