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

    }

    /// <summary>
    /// 单例
    /// </summary>
    public abstract class BaseRepositorySingleton
    {
        /// <summary>
        /// 
        /// </summary>
        private static IDbConnection _mesDbConnection;
        private static readonly object _lock = new();

        /// <summary>
        /// 构造函数（单例）
        /// </summary>
        /// <param name="connectionOptions"></param>
        protected BaseRepositorySingleton(IOptions<ConnectionOptions> connectionOptions)
        {
            if (_mesDbConnection == null)
            {
                lock (_lock)
                {
                    _mesDbConnection ??= new MySqlConnection(connectionOptions.Value.MESConnectionString);
                }
            }
        }

        /// <summary>
        /// MES库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetMESInstance()
        {
            // 处理获取实例失败的情况，抛出异常
            if (_mesDbConnection == null) throw new Exception("Failed to get instance of BaseRepository");
            return _mesDbConnection;
        }

    }
}
