using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace Hymson.MES.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class BaseRepository
    {
        /// <summary>
        /// 数据库连接（转子线）
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetRotorDbConnection()
        {
            var conn = new SqlConnection(_connectionOptions.RotorConnectionString);
            return conn;
        }

        /// <summary>
        /// 数据库连接（定子线）
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetStatorDbConnection()
        {
            var conn = new MySqlConnection(_connectionOptions.StatorConnectionString);
            return conn;
        }


    }

}
