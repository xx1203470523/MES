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
    /// 设备过程参数仓储
    /// </summary>
    public partial class ManuEquipmentParameterRepository : BaseRepository, IManuEquipmentParameterRepository
    {
        private readonly ParameterOptions _parameterOptions;
        public ManuEquipmentParameterRepository(IOptions<ConnectionOptions> connectionOptions, IOptions<ParameterOptions> parameterOptions) : base(connectionOptions)
        {
            _parameterOptions = parameterOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquipmentParameterEntity> list)
        {
            var dic = new Dictionary<string, List<EquipmentParameterEntity>>();
            foreach (var entity in list)
            {
                var tableName = GetTableNameByEquipmentId(entity.SiteId, entity.EquipmentId);
                if (!dic.ContainsKey(tableName))
                {
                    dic[tableName] = new List<EquipmentParameterEntity>();
                }
                dic[tableName].Add(entity);
            }
            using var conn = GetMESDbConnection();

            // 插入数据
            List<Task<int>> tasks = new();
            foreach (var dicItem in dic)
            {
                string insertSql = $"INSERT INTO {dicItem.Key}(`Id`, `SiteId`, `EquipmentId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId,@EquipmentId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted)";
                tasks.Add(conn.ExecuteAsync(insertSql, dicItem.Value));
            }
            var result = await Task.WhenAll(tasks);

            return result.Sum();
        }

        /// <summary>
        /// 获取建表SQL语句
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public string PrepareEquipmentParameterBySequenceTableSql(int sequence)
        {
            //获取目标表名
            var destinationTableName = $"{EquipmentParameter.EquipmentParameterPrefix}{sequence}";
            string createTableSql = $"CREATE TABLE `{destinationTableName}` LIKE `{EquipmentParameter.EquipmentProcedureParameterTemplateName}`;";
            return createTableSql;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<string> GetParamTableName(long siteId, long equipmentId)
        {
            var key = CalculateCrc32($"{siteId}{equipmentId}");

            return $"{EquipmentParameter.EquipmentParameterPrefix}{key % _parameterOptions.ParameterDelivery}";
        }

   /// <summary>
        /// 根据设备Id获取参数信息
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquipmentParameterEntity>> GetParametesByEqumentIdEntitiesAsync(ManuEquipmentParameterPagedQuery pagedQuery)
        {
            var equipmentId = pagedQuery.EquipmentId??0;
            var tableName = GetTableNameByEquipmentId(pagedQuery.SiteId, equipmentId);

            var sqlBuilder = new SqlBuilder();
            // WHERE EquipmentId=@EquipmentId  AND SiteId=@SiteId AND IsDeleted=0
            string getByEquipmentSql = $"SELECT Id, SiteId, EquipmentId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted FROM {tableName} /**where**/ LIMIT @Offset,@Rows ";
            string getByEquipmentCountSql = $"select count(1) from {tableName} /**where**/";
            var templateData = sqlBuilder.AddTemplate(getByEquipmentSql);
            var templateCount = sqlBuilder.AddTemplate(getByEquipmentCountSql);

            pagedQuery.EquipmentId= equipmentId;
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("EquipmentId = @EquipmentId");
            if (pagedQuery.ParameterId.HasValue)
            {
                sqlBuilder.Where("ParameterId=@ParameterId ");
            }

            //限定时间
            if (pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.CreatedOn[0], DateEnd = pagedQuery.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where(" CreatedOn >= @DateStart AND CreatedOn < @DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESParamterDbConnection();
            var entitiesTask = conn.QueryAsync<EquipmentParameterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquipmentParameterEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
        #region 内部方法
        /// <summary>
        /// 更具设备编码获取表名
        /// </summary>
        /// <param name="paran"></param>
        /// <returns></returns>
        private string GetTableNameByEquipmentId(long siteId, long equipmentId)
        {
            var key = CalculateCrc32($"{siteId}{equipmentId}");

            return $"{EquipmentParameter.EquipmentParameterPrefix}{key % _parameterOptions.ParameterDelivery}";
        }

        /// <summary>
        /// Crc32 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion
    }
}

