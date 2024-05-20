using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码关系表）
    /// </summary>
    public partial class ManuBarCodeRelationRepository : BaseRepository, IManuBarCodeRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuBarCodeRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuBarCodeRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuBarCodeRelationEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuBarCodeRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuBarCodeRelationEntity> entities)
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
        public async Task<ManuBarCodeRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuBarCodeRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBarCodeRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBarCodeRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBarCodeRelationEntity>> GetEntitiesAsync(ManuBarcodeRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();

            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (query.InputBarCodes != null && query.InputBarCodes.Any())
            {
                sqlBuilder.Where("InputBarCode IN @InputBarCodes");
            }
            if (query.IsDisassemble.HasValue)
            {
                sqlBuilder.Where("IsDisassemble=@IsDisassemble");
            }

            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBarCodeRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBarCodeRelationEntity>> GetPagedListAsync(ManuBarcodeRelationPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuBarCodeRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuBarCodeRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }



        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBarCodeRelationEntity>> GetSfcMoudulesAsync(ManuComponentBarcodeRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                sqlBuilder.Where("OutputBarCode = @Sfc");
            }

            if (query.IsDisassemble == SFCCirculationReportTypeEnum.Activity)
            {
                sqlBuilder.Where("IsDisassemble = 0");
            }

            if (query.IsDisassemble == SFCCirculationReportTypeEnum.Remove)
            {
                sqlBuilder.Where("IsDisassemble = 1");
            }

            if (!string.IsNullOrWhiteSpace(query.BomMainMaterialId))
            {
                sqlBuilder.Where("BusinessContent->>'$.BomMainMaterialId' = @BomMainMaterialId");
            }
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBarCodeRelationEntity>(templateData.RawSql, templateData.Parameters);
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBarCodeRelationEntity>> GetManuBarCodeRelationEntitiesAsync(ManuSfcProduceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (query.Sfcs != null && query.Sfcs.Any())
            {
                sqlBuilder.Where("OutputBarCode in @Sfcs");
            }
            using var conn = GetMESDbConnection();
            var manuSfcProduceEntities = await conn.QueryAsync<ManuBarCodeRelationEntity>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 条码关系表拆解移除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuBarCodeRelationUpdateAsync(DManuBarCodeRelationCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DisassemblyUpdateSql, command);
        }

        /// <summary>
        /// 根据Location查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBarCodeRelationEntity>> GetByLocationAsync(ManuComponentBarcodeRelationLocationQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBarCodeRelationEntity>(GetByLocationSql, query);
        }

        /// <summary>
        /// 条码关系表拆解移除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DisassemblyUpdateAsync(DisassemBarCodeRelationblyCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DisassemblyUpdateSql, command);
        }


        /// <summary>
        /// 组件使用报告 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBarCodeRelationEntity>> GetReportPagedInfoAsync(ComUsageReportPagedQuery queryParam)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where(" IsDeleted=0 ");
            sqlBuilder.Select("*");

            sqlBuilder.Where(" IsDisassemble=0 "); //筛选出未拆解的

            sqlBuilder.Where(" SiteId=@SiteId ");

            if (queryParam.CirculationProductId.HasValue)
            {
                sqlBuilder.Where(" InputBarCodeMaterialId=@CirculationProductId ");
            }

            if (queryParam.CreatedOn != null && queryParam.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = queryParam.CreatedOn[0], CreatedOnEnd = queryParam.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where(" CreatedOn >= @CreatedOnStart AND CreatedOn < @CreatedOnEnd ");
            }

            if (!string.IsNullOrEmpty(queryParam.CirculationBarCode))
            {
                sqlBuilder.Where(" InputBarCode=@CirculationBarCode ");
            }

            if (queryParam.ProcedureId.HasValue)
            {
                sqlBuilder.Where(" ProcedureId=@ProcedureId ");
            }

            if (queryParam.ResourceId.HasValue)
            {
                sqlBuilder.Where(" ResourceId=@ResourceId ");
            }


            var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
            sqlBuilder.AddParameters(queryParam);

            using var conn = GetMESDbConnection();
            var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuBarCodeRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuBarCodeRelationEntity>(manuSfcCirculationEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuBarCodeRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_barcode_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_barcode_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_barcode_relation /**where**/  ";

        const string InsertSql = "INSERT INTO manu_barcode_relation(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `InputBarcode`, `InputBarcodeLocation`, `InputBarcodeMaterialId`, `InputBarcodeWorkorderId`, `InputQty`, `OutputBarcode`, `OutputBarcodeMaterialId`, `OutputBarcodeWorkorderId`, `OutputBarcodeMode`, `RelationType`, `BusinessContent`, `IsDisassemble`, `DisassembledBy`, `DisassembledOn`, `SubstituteId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `DisassembledSfcStepId`) VALUES (  @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @InputBarcode, @InputBarcodeLocation, @InputBarcodeMaterialId, @InputBarcodeWorkorderId, @InputQty, @OutputBarcode, @OutputBarcodeMaterialId, @OutputBarcodeWorkorderId, @OutputBarcodeMode, @RelationType, @BusinessContent, @IsDisassemble, @DisassembledBy, @DisassembledOn, @SubstituteId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @DisassembledSfcStepId) ";
        const string InsertsSql = "INSERT INTO manu_barcode_relation(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `InputBarcode`, `InputBarcodeLocation`, `InputBarcodeMaterialId`, `InputBarcodeWorkorderId`, `InputQty`, `OutputBarcode`, `OutputBarcodeMaterialId`, `OutputBarcodeWorkorderId`, `OutputBarcodeMode`, `RelationType`, `BusinessContent`, `IsDisassemble`, `DisassembledBy`, `DisassembledOn`, `SubstituteId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `DisassembledSfcStepId`) VALUES (  @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @InputBarcode, @InputBarcodeLocation, @InputBarcodeMaterialId, @InputBarcodeWorkorderId, @InputQty, @OutputBarcode, @OutputBarcodeMaterialId, @OutputBarcodeWorkorderId, @OutputBarcodeMode, @RelationType, @BusinessContent, @IsDisassemble, @DisassembledBy, @DisassembledOn, @SubstituteId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @DisassembledSfcStepId) ";

        const string UpdateSql = "UPDATE manu_barcode_relation SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, InputBarcode = @InputBarcode, InputBarcodeLocation = @InputBarcodeLocation, InputBarcodeMaterialId = @InputBarcodeMaterialId, InputBarcodeWorkorderId = @InputBarcodeWorkorderId, InputQty = @InputQty, OutputBarcode = @OutputBarcode, OutputBarcodeMaterialId = @OutputBarcodeMaterialId, OutputBarcodeWorkorderId = @OutputBarcodeWorkorderId, OutputBarcodeMode = @OutputBarcodeMode, RelationType = @RelationType, BusinessContent = @BusinessContent, IsDisassemble = @IsDisassemble, DisassembledBy = @DisassembledBy, DisassembledOn = @DisassembledOn, SubstituteId = @SubstituteId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, DisassembledSfcStepId = @DisassembledSfcStepId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_barcode_relation SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, InputBarcode = @InputBarcode, InputBarcodeLocation = @InputBarcodeLocation, InputBarcodeMaterialId = @InputBarcodeMaterialId, InputBarcodeWorkorderId = @InputBarcodeWorkorderId, InputQty = @InputQty, OutputBarcode = @OutputBarcode, OutputBarcodeMaterialId = @OutputBarcodeMaterialId, OutputBarcodeWorkorderId = @OutputBarcodeWorkorderId, OutputBarcodeMode = @OutputBarcodeMode, RelationType = @RelationType, BusinessContent = @BusinessContent, IsDisassemble = @IsDisassemble, DisassembledBy = @DisassembledBy, DisassembledOn = @DisassembledOn, SubstituteId = @SubstituteId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, DisassembledSfcStepId = @DisassembledSfcStepId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_barcode_relation SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_barcode_relation SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_barcode_relation WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_barcode_relation WHERE Id IN @Ids ";

        const string DisassemblyUpdateSql = "UPDATE manu_barcode_relation SET " +
          "IsDisassemble = @IsDisassemble," +
          "DisassembledBy = @UserId, DisassembledOn = @UpdatedOn, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn, DisassembledSfcStepId = @DisassembledSfcStepId WHERE Id = @Id AND IsDisassemble <> @IsDisassemble ";

        const string GetByLocationSql = @"SELECT * FROM manu_barcode_relation WHERE SiteId = @SiteId AND InputBarCode = @SFC AND InputBarCodeLocation = @Location ";
    }
}
