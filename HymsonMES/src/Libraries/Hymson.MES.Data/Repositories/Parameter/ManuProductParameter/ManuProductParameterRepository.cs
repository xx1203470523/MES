using Dapper;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Hymson.MES.Data.Repositories.Parameter.ManuProductParameter
{
    /// <summary>
    /// 产品类型参数
    /// </summary>
    public partial class ManuProductParameterRepository : BaseRepository, IManuProductParameterRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        public ManuProductParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.ExecuteAsync(insertSql, list);
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterBySfcQuery param, string tableName)
        {
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted  FROM {tableName}  WHERE SFC IN @SFCs  AND SiteId =@SiteId AND IsDeleted=0";
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.QueryAsync<ManuProductParameterEntity>(getBySFCSql, param);
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
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.ExecuteAsync(updateSql, list);
        }

        /// <summary>
        /// 查询参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<string?> ShowCreateTableAsync(string tableName)
        {
            string sql = $"SHOW  CREATE TABLE {tableName}";
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            var result = await conn.QueryFirstOrDefaultAsync(sql);
            return ((ICollection<KeyValuePair<string, object>>)result).FirstOrDefault(x => x.Key == "Create Table").Value.ToString();
        }

        /// <summary>
        /// 创建数据库表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<int> CreateProductParameterTableAsync(string tableName)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return await conn.ExecuteAsync(tableName);
        }
    }
}

