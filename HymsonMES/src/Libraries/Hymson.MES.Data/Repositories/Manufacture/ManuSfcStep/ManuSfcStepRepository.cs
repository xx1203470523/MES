using Dapper;
using Force.Crc32;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤表仓储
    /// 有总表 有按条码分表
    /// </summary>
    public partial class ManuSfcStepRepository : BaseRepository, IManuSfcStepRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcStepRepository(IOptions<ConnectionOptions> connectionOptions, IOptions<ManuSfcStepTableOptions> options) : base(connectionOptions)
        {
            _options = options;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcStepEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(GetListByStartWaterMarkIdSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoAsync(ManuSfcStepPagedQuery manuSfcStepPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (manuSfcStepPagedQuery.PageIndex - 1) * manuSfcStepPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcStepPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcStepPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcStepEntitiesTask = conn.QueryAsync<ManuSfcStepEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepEntities = await manuSfcStepEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepEntity>(manuSfcStepEntities, manuSfcStepPagedQuery.PageIndex, manuSfcStepPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcStepQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetManuSfcStepEntitiesAsync(ManuSfcStepQuery manuSfcStepQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcStepEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted =0");
            sqlBuilder.Where("SiteId =@SiteId");
            if (manuSfcStepQuery.SFCs != null && manuSfcStepQuery.SFCs.Any())
            {
                sqlBuilder.Where("SFC  in @SFCs");
            }
            if (manuSfcStepQuery.Operatetype.HasValue)
            {
                sqlBuilder.Where("Operatetype=@Operatetype");
            }

            using var conn = GetMESDbConnection();
            var manuSfcStepEntities = await conn.QueryAsync<ManuSfcStepEntity>(template.RawSql, manuSfcStepQuery);
            return manuSfcStepEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcStepEntity manuSfcStepEntity)
        {
            using var conn = GetMESDbConnection();
            //插入主表数据
            await conn.ExecuteAsync(string.Format(InsertSql, PrepareTableName(manuSfcStepEntity)), manuSfcStepEntity);
            //插入分表数据
            return await conn.ExecuteAsync(string.Format(InsertSql, PrepareTableName(manuSfcStepEntity, false)), manuSfcStepEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcStepEntity>? manuSfcStepEntities)
        {
            if (manuSfcStepEntities == null || !manuSfcStepEntities.Any()) return 0;

            var keyValuePairs = TableGrouping(manuSfcStepEntities);
            using var conn = GetMESDbConnection();
            //插入分表数据
            foreach (var item in keyValuePairs)
            {
                await conn.ExecuteAsync(string.Format(InsertSql, item.Key), item.Value);
            }
            //插入主表数据
            return await conn.ExecuteAsync(string.Format(InsertSql, TEMPLATETABLENAME), manuSfcStepEntities);
        }

        /// <summary>
        /// 批量新增-不分表
        /// </summary>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeMavleAsync(IEnumerable<ManuSfcStepEntity>? manuSfcStepEntities)
        {
            if (manuSfcStepEntities == null || !manuSfcStepEntities.Any()) return 0;

            //var keyValuePairs = TableGrouping(manuSfcStepEntities);
            using var conn = GetMESDbConnection();
            ////插入分表数据
            //foreach (var item in keyValuePairs)
            //{
            //    await conn.ExecuteAsync(string.Format(InsertSql, item.Key), item.Value);
            //}
            //插入主表数据
            return await conn.ExecuteAsync(string.Format(InsertSql, TEMPLATETABLENAME), manuSfcStepEntities);
        }

        /// <summary>
        /// 获取马威数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetSfcStepMavelAsync(EntityByWaterSiteIdQuery query)
        {
            string rowSql = string.Empty;
            if(query.Rows != 0)
            {
                rowSql = "LIMIT @Rows";
            }

            string sql = $@"
                select * 
                from manu_sfc_step t1
                where t1.SiteId = @SiteId
                and Id > @StartWaterMarkId 
                ORDER BY Id ASC 
                {rowSql};
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(sql, query);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcStepEntity manuSfcStepEntity)
        {
            using var conn = GetMESDbConnection();
            //插入分表数据
            await conn.ExecuteAsync(string.Format(UpdateSql, PrepareTableName(manuSfcStepEntity, false)), manuSfcStepEntity);
            //插入主表数据
            return await conn.ExecuteAsync(string.Format(UpdateSql, PrepareTableName(manuSfcStepEntity)), manuSfcStepEntity);
        }

        #region 业务表
        /// <summary>
        /// 根据实体列表对数据进行按表名分组
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public Dictionary<string, IGrouping<string, ManuSfcStepEntity>> GetTableNames(IEnumerable<ManuSfcStepEntity> entities)
        {
            return entities.ToLookup(x => PrepareTableName(x, false)).ToDictionary(d => d.Key, d => d);
        }

        /// <summary>
        /// 获取SFC的进站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetInStationStepsBySFCAsync(EntityBySFCQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetInStepBySFCSql, PrepareTableName(query.SiteId, query.SFC, false)), query);
        }


        /// <summary>
        /// 获取SFC的进站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetStepsBySFCAsync(EntityBySFCQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetStepBySFCSql, PrepareTableName(query.SiteId, query.SFC, false)), query);
        }

        /// <summary>
        /// 获取SFC的出站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetOutStationStepsBySFCAsync(EntityBySFCQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetOutStepBySFCSql, PrepareTableName(query.SiteId, query.SFC, false)), query);
        }

        /// <summary>
        /// 获取SFC的进出站步骤
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetInOutStationStepsBySFCAsync(EntityBySFCQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetInOutStepBySFCSql, PrepareTableName(query.SiteId, query.SFC, false)), query);
        }

        /// <summary>
        /// 指定表情查询条码的进出站步骤
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetInOutStationStepsBySFCsAsync(string tableName, EntityBySFCsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetInOutStepBySFCsSql, tableName), query);
        }

        /// <summary>
        /// 根据条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetProductParameterBySFCEntitiesAsync(EntityBySFCsQuery param)
        {
            var list = new List<ManuSfcStepEntity>();
            var dic = new Dictionary<string, List<string>>();

            foreach (var sfc in param.SFCs)
            {
                var tableNameBySFC = PrepareTableName(param.SiteId, sfc, false);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<string>();
                }
                dic[tableNameBySFC].Add(sfc);
            }

            List<Task<IEnumerable<ManuSfcStepEntity>>> tasks = new();
            using var conn = GetMESDbConnection();
            foreach (var dicItem in dic)
            {
                tasks.Add(conn.QueryAsync<ManuSfcStepEntity>(string.Format(GetStepBySFCsSql, dicItem.Key), new EntityBySFCsQuery { SiteId = param.SiteId, SFCs = dicItem.Value }));
            }
            var result = await Task.WhenAll(tasks);
            foreach (var item in result)
            {
                list.AddRange(item);
            }
            return list;
        }

        /// <summary>
        /// 插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntitie"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcStepBusinessAsync(MaunSfcStepBusinessEntity maunSfcStepBusinessEntitie)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSfcStepBusinessSql, maunSfcStepBusinessEntitie);
        }

        /// <summary>
        /// 批量插入步骤业务表
        /// </summary>
        /// <param name="maunSfcStepBusinessEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcStepBusinessRangeAsync(IEnumerable<MaunSfcStepBusinessEntity> maunSfcStepBusinessEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSfcStepBusinessSql, maunSfcStepBusinessEntities);
        }
        #endregion

        /// <summary>
        /// 分页查询 根据SFC
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepEntity>> GetPagedInfoBySFCAsync(ManuSfcStepBySfcPagedQuery queryParam)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetBySFCPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetBySFCPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(queryParam.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }

            var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
            sqlBuilder.AddParameters(queryParam);

            using var conn = GetMESDbConnection();
            var manuSfcStepEntitiesTask = conn.QueryAsync<ManuSfcStepEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepEntities = await manuSfcStepEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepEntity>(manuSfcStepEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
        }

        /// <summary>
        /// 获取一些条码的所有进站信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepEntity>> GetSFCInStepAsync(SfcInStepQuery query)
        {
            using var conn = GetMESDbConnection();
            var manuSfcStepEntities = await conn.QueryAsync<ManuSfcStepEntity>(GetSfcsInStepSql, query);
            return manuSfcStepEntities;
        }

        /// <summary>
        /// 获取一个条码的合并新增或拆分新增步骤记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcStepEntity> GetSfcMergeOrSplitAddStepAsync(SfcMergeOrSplitAddStepQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepEntity>(GetSfcsMergeOrSliptAddStepSql, query);
        }

        public async Task<ManuSfcStepEntity> GetBarcodeBindingStepAsync(SfcMergeOrSplitAddStepQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepEntity>(GetBarcodeBindingStepSql, query);
        }

        #region private
        /// <summary>
        /// 模板表名称
        /// </summary>
        public const string TEMPLATETABLENAME = "manu_sfc_step";
        private readonly IOptions<ManuSfcStepTableOptions> _options;

        /// <summary>
        /// 根据站点id和条码计算分表索引值
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private uint CalculateTableIndex(long siteId, string sfc)
        {
            var crc32 = CalculateCrc32($"{siteId}{sfc}");
            return crc32 % _options.Value.Divides;
        }

        /// <summary>
        /// 根据实体列表对数据进行按表名分组
        /// </summary>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private Dictionary<string, IEnumerable<ManuSfcStepEntity>> TableGrouping(IEnumerable<ManuSfcStepEntity> manuSfcStepEntities)
        {
            Dictionary<string, IEnumerable<ManuSfcStepEntity>> keyValuePairs = new Dictionary<string, IEnumerable<ManuSfcStepEntity>>();
            foreach (var manuSfcStepEntity in manuSfcStepEntities)
            {
                var tableName = PrepareTableName(manuSfcStepEntity, false);
                if (!keyValuePairs.TryAdd(tableName, new List<ManuSfcStepEntity>() { manuSfcStepEntity }))
                {
                    keyValuePairs[tableName] = keyValuePairs[tableName].Append(manuSfcStepEntity).ToList();
                }
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 根据主表和分表拼接表名
        /// </summary>
        /// <param name="manuSfcStepEntity"></param>
        /// <param name="isMaster"></param>
        /// <returns></returns>
        private string PrepareTableName(ManuSfcStepEntity manuSfcStepEntity, bool isMaster = true)
        {
            if (isMaster)
                return TEMPLATETABLENAME;
            var tableIndex = CalculateTableIndex(manuSfcStepEntity.SiteId, manuSfcStepEntity.SFC);
            return $"{TEMPLATETABLENAME}_{tableIndex}";
        }

        /// <summary>
        /// 根据站点id和条码组装表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="sfc"></param>
        /// <param name="isMaster"></param>
        /// <returns></returns>
        private string PrepareTableName(long siteId, string sfc, bool isMaster = true)
        {
            if (isMaster)
                return TEMPLATETABLENAME;
            var tableIndex = CalculateTableIndex(siteId, sfc);
            return $"{TEMPLATETABLENAME}_{tableIndex}";
        }

        /// <summary>
        /// crc32算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion  

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcStepRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_step` /**where**/ ";
        const string GetManuSfcStepEntitiesSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step` /**where**/  ";
        const string InsertSfcStepBusinessSql = "INSERT INTO `manu_sfc_step_business`(`Id`, `SiteId`, `SfcStepId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcStepId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertSql = "INSERT INTO  `{0}` (  `Id`, `SFC`, `ProductId`, `WorkOrderId`, `WorkCenterId`, `ProductBOMId`, `ProcessRouteId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `VehicleCode`, `IsRepair`, `CurrentStatus`, `Operatetype`, `OperationProcedureId`, `OperationResourceId`, `OperationEquipmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `AfterOperationStatus`) VALUES (  @Id, @SFC, @ProductId, @WorkOrderId, @WorkCenterId, @ProductBOMId,@ProcessRouteId, @Qty, @EquipmentId, @ResourceId, @ProcedureId, @VehicleCode, @IsRepair, @CurrentStatus, @Operatetype, @OperationProcedureId, @OperationResourceId, @OperationEquipmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @AfterOperationStatus) ";
        const string UpdateSql = "UPDATE `{0}` SET   SFC = @SFC, ProductId = @ProductId, WorkOrderId = @WorkOrderId, WorkCenterId = @WorkCenterId, ProductBOMId = @ProductBOMId, Qty = @Qty, EquipmentId = @EquipmentId, ResourceId = @ResourceId, ProcedureId = @ProcedureId, VehicleCode = @VehicleCode, IsRepair = @IsRepair, CurrentStatus = @CurrentStatus, Operatetype = @Operatetype, OperationProcedureId = @OperationProcedureId, OperationResourceId = @OperationResourceId, OperationEquipmentId = @OperationEquipmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, AfterOperationStatus = @AfterOperationStatus WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT * FROM `manu_sfc_step`  WHERE Id = @Id ";
        const string GetListByStartWaterMarkIdSql = @"SELECT * FROM `manu_sfc_step` WHERE Id > @StartWaterMarkId ORDER BY Id ASC LIMIT @Rows";
        const string GetStepBySFCSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId  AND SFC = @SFC ORDER BY Id ASC ";
        const string GetInStepBySFCSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype = 3 AND SFC = @SFC ORDER BY Id ASC ";
        const string GetOutStepBySFCSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype = 4 AND SFC = @SFC ORDER BY Id ASC ";
        const string GetInOutStepBySFCSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype IN (3, 4) AND SFC = @SFC ORDER BY Id ASC ";
        const string GetInOutStepBySFCsSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype IN (3, 4) AND SFC IN @SFCs ORDER BY Id ASC ";
        const string GetStepBySFCsSql = @"SELECT * FROM `{0}` WHERE IsDeleted = 0 AND SiteId = @SiteId AND  SFC IN @SFCs ORDER BY Id DESC ";
        const string GetBySFCPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY Id desc LIMIT @Offset, @Rows ";
        const string GetBySFCPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_step` /**where**/ ";

        /// <summary>
        /// 获取条码的进站信息
        /// </summary>
        const string GetSfcsInStepSql = @"SELECT * FROM  manu_sfc_step WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype = 3 AND sfc IN @sfcs ";

        /// <summary>
        /// 获取条码的合并新增或拆分新增信息
        /// </summary>
        const string GetSfcsMergeOrSliptAddStepSql = @"SELECT * FROM  manu_sfc_step WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype in (39,42) AND sfc = @Sfc ";
        const string GetBarcodeBindingStepSql = @"SELECT * FROM  manu_sfc_step WHERE IsDeleted = 0 AND SiteId = @SiteId AND Operatetype = 27 AND sfc = @Sfc ";
    }

}
