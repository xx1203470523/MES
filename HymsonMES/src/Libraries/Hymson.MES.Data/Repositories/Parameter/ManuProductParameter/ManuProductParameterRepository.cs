using Dapper;
using Force.Crc32;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Parameter.ManuProductParameter
{
    /// <summary>
    /// 产品类型参数
    /// </summary>
    public partial class ManuProductParameterRepository : BaseRepository, IManuProductParameterRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        private readonly ParameterOptions _parameterOptions;

        public ManuProductParameterRepository(IOptions<ConnectionOptions> connectionOptions, IOptions<ParameterOptions> parameterOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
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

            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            List<Task<int>> tasks = new();
            foreach (var dicItem in dic)
            {
                string insertSql = $"INSERT INTO {dicItem.Key}(`Id`, `SiteId`, `SFC`, `ProcedureId`, `ParameterId`, `ParameterValue`, `CollectionTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @SFC,@ProcedureId, @ParameterId,@ParameterValue,@CollectionTime,@CreatedBy,@CreatedOn, @UpdatedBy, @UpdatedOn,@IsDeleted)";
                tasks.Add(conn.ExecuteAsync(insertSql, dicItem.Value));
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
        /// 更具条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterBySfcQuery param)
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
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
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
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterEntities(ManuProductParameterByProcedureIdQuery param)
        {
            var tableNameByProcedureId = GetTableNameByProcedureId(param.SiteId, param.ProcedureId);
            string getBySFCSql = $"SELECT Id, SiteId, SFC, ProcedureId, ParameterId, ParameterValue, CollectionTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted  FROM {tableNameByProcedureId}  WHERE SFC IN @SFCs AND  ProcedureId=@ProcedureId  AND SiteId =@SiteId AND IsDeleted=0";
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
          
            using var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
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

        /// <summary>
        /// SHA256 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private BigInteger CalculateSHA256Hash(string input)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = hasher.ComputeHash(inputBytes);

                BigInteger hashValue = new BigInteger(hashBytes);

                return hashValue;
            }
        }

        private uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion
    }
}

