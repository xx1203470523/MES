using Dapper;
using Hymson.MES.Data.Repositories.Common.Query;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class OPRepository<TEntity> : BaseRepository, IOPRepository<TEntity> where TEntity : BaseOPEntity
    {
        /// <summary>
        /// 水位查询SQL
        /// </summary>
        //const string QuerySql = @"SELECT * FROM `{0}` WHERE RDate >= '2024-08-11 08:30:00' AND `index` > @StartWaterMarkId ORDER BY `index` ASC LIMIT @Rows";
        const string QuerySql = @"SELECT * FROM `{0}` WHERE `index` > @StartWaterMarkId ORDER BY `index` ASC LIMIT @Rows";
        const string QueryByIdSql = @"SELECT * FROM `{0}` WHERE `ID` IN @Ids ORDER BY `index` ASC";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public OPRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<TEntity>(string.Format(QueryByIdSql, typeof(TEntity).Name), new { Ids = ids });
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<TEntity>(string.Format(QuerySql, typeof(TEntity).Name), query);
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query, string tableName)
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<TEntity>(string.Format(QuerySql, tableName), query);
        }

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataTable> GetDataTableByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetStatorDbConnection();
            using var reader = await conn.ExecuteReaderAsync(string.Format(QuerySql, typeof(TEntity).Name), query);

            var dt = new DataTable { };
            dt.Load(reader);

            return dt;
        }

        /// <summary>
        /// 将DataTable转为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertDataTableToList<T>(DataTable dt)
        {
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable()
                     .Select(row => (T)properties.Aggregate(Activator.CreateInstance<T>(),
                             (current, prop) =>
                             {
                                 prop.SetValue(current, row[prop.Name]);
                                 return current;
                             }));

            /*
            List<T> data = new();
            foreach (DataRow row in dt.Rows)
            {
                T item = new();
                foreach (PropertyInfo prop in item.GetType().GetProperties())
                {
                    if (dt.Columns.Contains(prop.Name))
                    {
                        prop.SetValue(item, row[prop.Name], null);
                    }
                }
                data.Add(item);
            }
            return data;
            */

            /*
            return dt.AsEnumerable()
             .Select(row => Activator.CreateInstance<T>()
             .GetType()
             .GetProperties()
             .Aggregate((T)new object(), (current, prop) =>
             {
                 prop.SetValue(current, row[prop.Name]);
                 return current;
             }))
             .Cast<T>();
            */
        }

    }
}
