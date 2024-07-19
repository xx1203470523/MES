using Dapper;
using Force.Crc32;
using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

using System.Text;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Parameter
{
    /// <summary>
    /// 参数收集仓储
    /// </summary>
    public partial class ManuProductParameterRepository : BaseRepository, IManuProductParameterRepository
    {
        private readonly ParameterOptions _parameterOptions;

        public ManuProductParameterRepository(IOptions<ConnectionOptions> connectionOptions, IOptions<ParameterOptions> parameterOptions) : base(connectionOptions)
        {
            _parameterOptions = parameterOptions.Value;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuProductParameterEntity> entities)
        {
            if (entities == null || entities.Any() == false) return 0;

            // TODO 

            await Task.CompletedTask;
            return 0;

            /*
            using var conn = GetMESDbConnection();
            StringBuilder stringBuilder = new StringBuilder(InsertHeadSql);
            var insertsParams = new DynamicParameters();
            var i = 0;
            foreach (var item in entities)
            {
                stringBuilder.AppendFormat(InsertTailSql, i);
                insertsParams.Add($"{nameof(item.Id)}{i}", item.Id);
                insertsParams.Add($"{nameof(item.SiteId)}{i}", item.SiteId);
                insertsParams.Add($"{nameof(item.ProcedureId)}{i}", item.ProcedureId);
                insertsParams.Add($"{nameof(item.ResourceId)}{i}", item.ResourceId);
                insertsParams.Add($"{nameof(item.EquipmentId)}{i}", item.EquipmentId);
                insertsParams.Add($"{nameof(item.SFC)}{i}", item.SFC);
                insertsParams.Add($"{nameof(item.WorkOrderId)}{i}", item.WorkOrderId);
                insertsParams.Add($"{nameof(item.ProductId)}{i}", item.ProductId);
                insertsParams.Add($"{nameof(item.ParameterId)}{i}", item.ParameterId);
                insertsParams.Add($"{nameof(item.ParamValue)}{i}", item.ParamValue);
                insertsParams.Add($"{nameof(item.StandardUpperLimit)}{i}", item.StandardUpperLimit);
                insertsParams.Add($"{nameof(item.StandardLowerLimit)}{i}", item.StandardLowerLimit);
                insertsParams.Add($"{nameof(item.JudgmentResult)}{i}", item.JudgmentResult);
                insertsParams.Add($"{nameof(item.TestDuration)}{i}", item.TestDuration);
                insertsParams.Add($"{nameof(item.TestTime)}{i}", item.TestTime);
                insertsParams.Add($"{nameof(item.TestResult)}{i}", item.TestResult);
                insertsParams.Add($"{nameof(item.LocalTime)}{i}", item.LocalTime);
                insertsParams.Add($"{nameof(item.CreatedBy)}{i}", item.CreatedBy);
                insertsParams.Add($"{nameof(item.CreatedOn)}{i}", item.CreatedOn);
                insertsParams.Add($"{nameof(item.UpdatedBy)}{i}", item.UpdatedBy);
                insertsParams.Add($"{nameof(item.UpdatedOn)}{i}", item.UpdatedOn);
                insertsParams.Add($"{nameof(item.IsDeleted)}{i}", item.IsDeleted);
                insertsParams.Add($"{nameof(item.StepId)}{i}", item.StepId);
                i++;

            }
            stringBuilder.Length--;
            return await conn.ExecuteAsync(stringBuilder.ToString(), insertsParams);
            */
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(EquipmentIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteScalarAsync(IsExistsSql, query) != null;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductParameterEntity> list, string tableName)
        {
            string insertSql = $"INSERT INTO {tableName}(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,  `SfcstepId`) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted,@SfcstepId)";
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(insertSql, list);
        }

        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductParameterEntity> list)
        {
            var dic = new Dictionary<string, List<ManuProductParameterEntity>>();

            foreach (var entity in list)
            {
                var tableNameBySFC = GetTableNameBySFC(entity.SiteId, entity.SFC);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<ManuProductParameterEntity>();
                }
                dic[tableNameBySFC].Add(entity);

                var tableNameByProcedureId = GetTableNameByProcedureId(entity.SiteId, entity.ProcedureId);

                if (!dic.ContainsKey(tableNameByProcedureId))
                {
                    dic[tableNameByProcedureId] = new List<ManuProductParameterEntity>();
                }
                dic[tableNameByProcedureId].Add(entity);
            }

            using var conn = GetMESParamterDbConnection();
            List<Task<int>> tasks = new();
            foreach (var dicItem in dic)
            {
                string insertSql = $"INSERT INTO {dicItem.Key}(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,  `SfcstepId`,`ResourceId`,`ParameterGroupId` ) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted,@SfcstepId,@ResourceId,@ParameterGroupId)";
                tasks.Add(conn.ExecuteAsync(insertSql, dicItem.Value));
            }
            var result = await Task.WhenAll(tasks);

            return result.Sum();
        }

        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeMavelAsync(IEnumerable<ManuProductParameterEntity> list)
        {
            var dic = new Dictionary<string, List<ManuProductParameterEntity>>();

            using var conn = GetMESParamterDbConnection();
            List<Task<int>> tasks = new();

            int maxNum = 999;
            int batchNum = list.Count() / maxNum + 1;
            for (var i = 0; i < batchNum; ++i)
            {
                List<ManuProductParameterEntity> curList = list.Skip(i * maxNum).Take(maxNum).ToList();
                string insertSql = $"INSERT INTO manu_product_procedure_parameter(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,  `SfcstepId`,`ResourceId`,`ParameterGroupId` ) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted,@SfcstepId,@ResourceId,@ParameterGroupId)";
                tasks.Add(conn.ExecuteAsync(insertSql, curList));
            }

            var result = await Task.WhenAll(tasks);

            return result.Sum();
        }

        /// <summary>
        /// 获取马威参数
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetManuParamMavelAsync(EntityByWaterMarkQuery query)
        {
            string sql = $@"
                select * 
                from manu_product_procedure_parameter
                WHERE Id > @StartWaterMarkId 
                ORDER BY Id ASC 
                LIMIT @Rows"";
            ";

            using var conn = GetMESParamterDbConnection();
            return await conn.QueryAsync<ManuProductParameterEntity>(sql, query);
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntitiesAsync(ManuProductParameterBySfcQuery param, string tableName)
        {
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SfcstepId  FROM {tableName}  WHERE SFC IN @SFCs  AND SiteId =@SiteId AND IsDeleted=0";
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryAsync<ManuProductParameterEntity>(getBySFCSql, param);
        }

        /// <summary>
        /// 根据条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterBySFCEntitiesAsync(ManuProductParameterBySfcQuery param)
        {
            var list = new List<ManuProductParameterEntity>();
            var dic = new Dictionary<string, List<string>>();

            foreach (var sfc in param.SFCs)
            {
                var tableNameBySFC = GetTableNameBySFC(param.SiteId, sfc);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<string>();
                }
                dic[tableNameBySFC].Add(sfc);
            }

            List<Task<IEnumerable<ManuProductParameterEntity>>> tasks = new();
            using var conn = GetMESParamterDbConnection();
            foreach (var dicItem in dic)
            {
                string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted , SfcstepId  FROM {dicItem.Key}  WHERE SFC IN @SFCs  AND SiteId =@SiteId AND IsDeleted=0";
                tasks.Add(conn.QueryAsync<ManuProductParameterEntity>(getBySFCSql, param));
            }
            var result = await Task.WhenAll(tasks);
            foreach (var item in result)
            {
                list.AddRange(item);
            }
            return list;
        }

        /// <summary>
        /// 更具工序获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterByProcedureIdEntitiesAsync(ManuProductParameterByProcedureIdQuery param)
        {
            var tableNameByProcedureId = GetTableNameByProcedureId(param.SiteId, param.ProcedureId);
            var sqlBuilder = new SqlBuilder();
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SfcstepId   FROM {tableNameByProcedureId}  /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
            var templateData = sqlBuilder.AddTemplate(getBySFCSql);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId =@SiteId");
            sqlBuilder.Where("ProcedureId = @ProcedureId");
            if (param.SFCs != null && param.SFCs.Any())
            {
                sqlBuilder.Where("SFC = @SFCs");
            }
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryAsync<ManuProductParameterEntity>(templateData.RawSql, param);
        }

        /// <summary>
        /// 修改参数值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterUpdateCommand> list, string tableName)
        {
            string updateSql = $"UPDATE {tableName} SET   ParameterValue = @ParameterValue, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE Id = @Id AND IsDeleted=0";
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(updateSql, list);
        }

        /// <summary>
        /// 修改参数值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductParameterUpdateCommand> list)
        {
            var dic = new Dictionary<string, List<ManuProductParameterUpdateCommand>>();
            foreach (var command in list)
            {
                var tableNameBySFC = GetTableNameBySFC(command.SiteId, command.SFC);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<ManuProductParameterUpdateCommand>();
                }
                dic[tableNameBySFC].Add(command);

                var tableNameByProcedureId = GetTableNameByProcedureId(command.SiteId, command.ProcedureId);

                if (!dic.ContainsKey(tableNameByProcedureId))
                {
                    dic[tableNameByProcedureId] = new List<ManuProductParameterUpdateCommand>();
                }
                dic[tableNameByProcedureId].Add(command);
            }

            using var conn = GetMESParamterDbConnection();
            List<Task<int>> tasks = new();
            foreach (var dicItem in dic)
            {
                string updateSql = $"UPDATE {dicItem.Key} SET   ParameterValue = @ParameterValue, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE Id = @Id AND IsDeleted=0";
                tasks.Add(conn.ExecuteAsync(updateSql, dicItem.Value));
            }
            var result = await Task.WhenAll(tasks);

            var row = 0;
            foreach (var item in result)
            {
                row += item;
            }
            return row;
        }

        /// <summary>
        /// 准备工序维度创建数据库表sql语句
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public string PrepareProductParameterProcedureIdTableSql(long siteId, long procedureId)
        {
            //获取目标表名
            var destinationTableName = GetTableNameByProcedureId(siteId, procedureId);
            string createTableSql = $"CREATE TABLE `{destinationTableName}` LIKE `{ProductParameter.ProductProcedureParameterTemplateName}`;";
            return createTableSql;
        }

        /// <summary>
        /// 根据工序或者条码分页查询产品参数
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductParameterEntity>> GetParametesEntitiesAsync(ManuProductParameterPagedQuery pagedQuery)
        {
            var procedureId = pagedQuery.ProcedureId ?? 0;
            var sfc = pagedQuery.Sfc ?? "";
            var tableName = "";
            if (procedureId > 0)
            {
                tableName = GetTableNameByProcedureId(pagedQuery.SiteId, procedureId);
            }
            else
            {
                tableName = GetTableNameBySFC(pagedQuery.SiteId, sfc);
            }

            var sqlBuilder = new SqlBuilder();
            string getByEquipmentSql = $"SELECT Id, SiteId, SFC, ProcedureId,ParameterId,ParameterValue,CollectionTime,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted,SfcstepId,ResourceId,ParameterGroupId FROM {tableName} /**where**/ order by Id DESC LIMIT @Offset,@Rows ";
            string getByEquipmentCountSql = $"select count(1) from {tableName} /**where**/";
            var templateData = sqlBuilder.AddTemplate(getByEquipmentSql);
            var templateCount = sqlBuilder.AddTemplate(getByEquipmentCountSql);

            pagedQuery.ProcedureId = procedureId;
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            //工序
            if (procedureId > 0)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }

            //条码
            if (!string.IsNullOrWhiteSpace(pagedQuery.Sfc))
            {
                sqlBuilder.Where("SFC=@SFC");
            }

            //参数
            if (pagedQuery.ParameterId.HasValue)
            {
                sqlBuilder.Where("ParameterId=@ParameterId ");
            }

            //条码列表
            if (pagedQuery.Sfcs != null && pagedQuery.Sfcs.Any())
            {
                sqlBuilder.Where("Sfc in @Sfcs ");
            }

            //限定时间
            if (pagedQuery.CollectionTimeRange != null && pagedQuery.CollectionTimeRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.CollectionTimeRange[0], DateEnd = pagedQuery.CollectionTimeRange[1] });
                sqlBuilder.Where(" CollectionTime>= @DateStart AND CollectionTime<@DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESParamterDbConnection();
            var entitiesTask = conn.QueryAsync<ManuProductParameterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductParameterEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        public Task<int> PrepareProductParameterSFCTable(int index)
        {
            //获取目标表名
            var destinationTableName = ProductParameter.ProductParameterPrefix + index;
            string createTableSql = $"CREATE TABLE `{destinationTableName}` LIKE `{ProductParameter.ProductProcedureParameterTemplateName}`;";
            using var conn = GetMESParamterDbConnection();
            return conn.ExecuteScalarAsync<int>(createTableSql, null);
        }

        public Task<int> PrepareProductParameterProcedureldTable(long siteId, long procedureId)
        {
            //获取目标表名
            var destinationTableName = GetTableNameByProcedureId(siteId, procedureId);
            string createTableSql = $"CREATE TABLE `{destinationTableName}` LIKE `{ProductParameter.ProductProcedureParameterTemplateName}`;";
            using var conn = GetMESParamterDbConnection();
            return conn.ExecuteScalarAsync<int>(createTableSql, null);
        }

        #region 内部方法
        /// <summary>
        /// 更具SFC获取表名
        /// </summary>
        /// <param name="paran"></param>
        /// <returns></returns>
        private string GetTableNameBySFC(long siteId, string sfc)
        {
            var key = CalculateCrc32($"{siteId}{sfc}");
            return $"{ProductParameter.ProductParameterPrefix}{key % _parameterOptions.ParameterDelivery}";
        }

        /// <summary>
        /// 更具工序编码获取表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>.
        private string GetTableNameByProcedureId(long siteId, long procedureId)
        {
            var key = CalculateCrc32($"{siteId}{procedureId}");

            return $"{ProductParameter.ProductProcedureParameterPrefix}{key}";
        }

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
    public partial class ManuProductParameterRepository
    {
        const string InsertHeadSql = @"INSERT INTO `manu_product_parameter`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `ParameterId`, `ParamValue`, StandardUpperLimit, StandardLowerLimit, JudgmentResult, TestDuration, TestTime, TestResult,`LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `StepId`) 
                        VALUES ";
        const string InsertTailSql = @"(@Id{0}, @SiteId{0}, @ProcedureId{0}, @ResourceId{0}, @EquipmentId{0}, @SFC{0}, @WorkOrderId{0}, @ProductId{0}, @ParameterId{0}, @ParamValue{0}, @StandardUpperLimit{0}, @StandardLowerLimit{0}, @JudgmentResult{0}, @TestDuration{0}, @TestTime{0}, @TestResult{0}, @LocalTime{0}, @CreatedBy{0}, @CreatedOn{0}, @UpdatedBy{0}, @UpdatedOn{0}, @IsDeleted{0}, @StepId{0}),";

        const string IsExistsSql = @"SELECT Id FROM manu_product_parameter WHERE `IsDeleted` = 0 AND SiteId = @SiteId AND EquipmentId = @EquipmentId AND ResourceId = @ResourceId AND SFC = @SFC LIMIT 1";

    }

}

