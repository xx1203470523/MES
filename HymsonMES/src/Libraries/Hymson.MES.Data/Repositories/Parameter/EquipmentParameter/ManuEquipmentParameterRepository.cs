using Dapper;
using Force.Crc32;
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

