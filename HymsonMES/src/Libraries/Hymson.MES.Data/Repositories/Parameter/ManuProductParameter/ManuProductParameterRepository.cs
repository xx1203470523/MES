using Dapper;
using Force.Crc32;
using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Options;
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
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductParameterEntity> list, string tableName)
        {
            string insertSql = $"INSERT INTO {tableName}(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted)";
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
                string insertSql = $"INSERT INTO {dicItem.Key}(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted)";
                tasks.Add(conn.ExecuteAsync(insertSql, dicItem.Value));
            }
            var result = await Task.WhenAll(tasks);

            return result.Sum();
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntitiesAsync(ManuProductParameterBySfcQuery param, string tableName)
        {
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted  FROM {tableName}  WHERE SFC IN @SFCs  AND SiteId =@SiteId AND IsDeleted=0";
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
                string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted  FROM {dicItem.Key}  WHERE SFC IN @SFCs  AND SiteId =@SiteId AND IsDeleted=0";
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
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted  FROM {tableNameByProcedureId}  /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
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
        /// 获取参数表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public List<string> GetParamTableName(long siteId, long procedureId, string sfc)
        {
            List<string> sfcList = new List<string>
            {
                GetTableNameBySFC(siteId, sfc),
                GetTableNameByProcedureId(siteId,procedureId)
            };
            return sfcList;
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
            string getByEquipmentSql = $"SELECT Id, SiteId, SFC, ProcedureId,ParameterId,ParameterValue,CollectionTime,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted FROM {tableName} /**where**/ order by Id DESC LIMIT @Offset,@Rows ";
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
}

