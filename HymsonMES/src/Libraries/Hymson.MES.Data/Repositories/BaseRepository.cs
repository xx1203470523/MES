#if DM
using Dm;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System.Data;
#else
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
#endif

namespace Hymson.MES.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
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
#if DM
            var conn = new DmConnection(_connectionOptions.MESConnectionString);
#else
            var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
#endif
            return conn;
        }

        /// <summary>
        /// MESParamter库连接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetMESParamterDbConnection()
        {
#if DM
            var conn = new DmConnection(_connectionOptions.MESParamterConnectionString);
#else
            var conn = new MySqlConnection(_connectionOptions.MESParamterConnectionString);
#endif
            return conn;
        }

        /// <summary>
        /// DorisParamter库连接
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetDorisParamterDbConnection()
        {
#if DM
            var conn = new DmConnection(_connectionOptions.DorisParamterConnectionString);
#else
            var conn = new MySqlConnection(_connectionOptions.DorisParamterConnectionString);
#endif
            return conn;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepositorySingleton<T> : IDisposable where T : BaseRepositorySingleton<T>, new()
    {
#if DM
        private readonly Lazy<DmConnection> _connection;
#else
        private readonly Lazy<MySqlConnection> _connection;
#endif

        private readonly IOptions<ConnectionOptions> _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public BaseRepositorySingleton(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions;
#if DM
            _connection = new Lazy<DmConnection>(() => new DmConnection(_connectionOptions.Value.MESConnectionString));
#else
            _connection = new Lazy<MySqlConnection>(() => new MySqlConnection(_connectionOptions.Value.MESConnectionString));
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<IDbConnection> GetDbConnectionAsync()
        {
            await OpenAsync();
            return _connection.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task OpenAsync()
        {
            if (_connection.Value.State == ConnectionState.Closed)
            {
                await _connection.Value.OpenAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
