using Dapper;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Parameter.ManuProductParameter
{
    /// <summary>
    /// 产品类型参数
    /// </summary>
    public partial class ManuEquipmentParameterRepository : BaseRepository, IManuEquipmentParameterRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        public ManuEquipmentParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquipmentParameterEntity> list, string tableName)
        {
            string insertSql = $"INSERT INTO {tableName}(`Id`, `SiteId`, `EquipmentId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId,@EquipmentId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted)";
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.ExecuteAsync(insertSql, list);
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<string?> ShowCreateTable(string tableName)
        {
            string sql = $"SHOW  CREATE TABLE {tableName}";
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            var result = await conn.QueryFirstOrDefaultAsync(sql);
            return ((ICollection<KeyValuePair<string, object>>)result).FirstOrDefault(x => x.Key == "Create Table").Value.ToString();
        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableSql"></param>
        /// <returns></returns>
        public async Task<int> CreateProductParameterTable(string tableSql)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.ExecuteAsync(tableSql);
        }
    }
}

