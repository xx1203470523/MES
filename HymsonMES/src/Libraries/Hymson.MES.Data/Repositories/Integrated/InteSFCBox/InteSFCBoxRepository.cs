﻿using Dapper;
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
    public partial class InteSFCBoxRepository: BaseRepository, IInteSFCBoxRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        public InteSFCBoxRepository (IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
            if (pagedQuery.Status!=null && pagedQuery.Status.HasValue)
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

        public async Task<PagedInfo<InteSFCBoxEntity>> GetBoxCodeAsync(InteSFCBoxQueryRep pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetBoxCodeSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetBoxCodeCountSqlTemplate);
            sqlBuilder.Where("msb.IsDeleted = 0");
            sqlBuilder.Where("msb.SiteId = @SiteId");
            //sqlBuilder.OrderBy("msb.CreateOn DESC");

            //sqlBuilder.Select("IC.Id, IC.Remark, IC.Status, IC.DefinitionMethod, IC.Level, IC.Maximum, IC.Minimum, IC.UpdatedBy, IC.UpdatedOn");
            //sqlBuilder.Select("(CASE IC.DefinitionMethod WHEN 1 THEN M.MaterialCode WHEN 2 THEN MG.GroupCode ELSE '' END) AS Name, M.Version");
            //sqlBuilder.LeftJoin("proc_material M ON IC.MaterialId = M.Id");
            //sqlBuilder.LeftJoin("proc_material_group MG ON IC.MaterialGroupId = MG.Id");
            if (!string.IsNullOrWhiteSpace(pagedQuery.BoxCode))                
            {
                sqlBuilder.Where("msb.BoxCode = @BoxCode");
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

        /// <summary>
        /// 批量查询BoxCode
        /// </summary>
        /// <param name="boxCodes"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSFCAllView>> GetByBoxCodesAsync(string[] boxCodes)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteSFCAllView>(GetByBoxCodesSql, new { BoxCodes = boxCodes });
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
        const string InsertsSql = "INSERT INTO `manu_sfc_box`(  `Id`, `SiteId`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness`) VALUES (   @Id, @SiteId, @SFC, @BoxCode, @Grade, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy,@UpdatedOn, @IsDeleted, @OCVB,@OCVBDate, @Weight, @DC, @DCDate, @IMPB, @SelfDischargeRate, @Width, @HeightZ, @HeightF,@ShoulderHeightZ, @ShoulderHeightF, @Thickness)  ";
        const string InsertsWorkOrderSql = "INSERT INTO `manu_sfc_box_workorder`( `Id`,`SiteId`,`BoxCode`,`WorkOrderId`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (   @Id,@SiteId,@BoxCode,@WorkOrderId,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted)  ";
        const string DeleteWorkOrderSql = "delete FROM manu_sfc_box_workorder where WorkOrderId=@WorkOrderId";

        const string GetPagedInfoDataSqlTemplate = @"SELECT `Id`, `SiteId`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness` FROM manu_sfc_box /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
       
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc_box /**where**/";

        const string GetBoxCodeSqlTemplate = @"SELECT `Id`, `SiteId`, `SFC`, `BoxCode`, `Grade`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `OCVB`, `OCVBDate`, `Weight`, `DC`, `DCDate`, `IMPB`, `SelfDischargeRate`, `Width`, `HeightZ`, `HeightF`, `ShoulderHeightZ`, `ShoulderHeightF`, `Thickness` FROM manu_sfc_box WHERE id in(
            SELECT MIN(id) FROM manu_sfc_box msb  /**where**/  GROUP BY BoxCode HAVING COUNT(1)>1
            ) LIMIT @Offset,@Rows ";
        const string GetBoxCodeCountSqlTemplate = @"SELECT COUNT(*) FROM (SELECT DISTINCT BoxCode FROM manu_sfc_box msb  /**where**/ ) as subquery";

        const string GetByBoxCodesSql = @"SELECT ROUND(max(OCVB),4)*10000-ROUND(min(OCVB),4)*10000 as OCVBDiff,round(max(IMPB),2) as MaxIMPB,BoxCode FROM `manu_sfc_box`  WHERE BoxCode IN @BoxCodes group  by BoxCode";

        const string GetWorkOrderIdSql = @"select  * from manu_sfc_box_workorder where WorkOrderId =@WorkOrderId";

        const string GetByIdsSql = @"SELECT * FROM `manu_sfc_box`  WHERE Id IN @Ids ";

        const string DeleteSql = "UPDATE manu_sfc_box SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
    }

}
