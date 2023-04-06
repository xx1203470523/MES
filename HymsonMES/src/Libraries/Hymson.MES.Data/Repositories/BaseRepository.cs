using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Hymson.MES.Data.Repositories
{
    public abstract class BaseRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        protected BaseRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }
        /// <summary>
        /// MES库连接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetMESDbConnection()
        {
            var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return conn;
        }

    }
}
