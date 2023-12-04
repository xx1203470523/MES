using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Hymson.MES.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// MESParamter库连接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetMESParamterDbConnection()
        {
            var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
            return conn;
        }

        /// <summary>
        /// DorisParamter库连接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetDorisParamterDbConnection()
        {
            var conn = new MySqlConnection(_connectionOptions.DorisParamterConnectionString);
            return conn;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepositorySingleton<T> : IDisposable where T : BaseRepositorySingleton<T>, new()
    {
        private readonly Lazy<MySqlConnection> _connection;
        private readonly IOptions<ConnectionOptions> _connectionOptions;

        public BaseRepositorySingleton(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions;
            _connection = new Lazy<MySqlConnection>(() => new MySqlConnection(_connectionOptions.Value.MESConnectionString));
        }


        protected async Task<IDbConnection> GetDbConnectionAsync()
        {
            await OpenAsync();
            return _connection.Value;
        }

        private async Task OpenAsync()
        {
            if (_connection.Value.State == ConnectionState.Closed)
            {
                await _connection.Value.OpenAsync();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

}
