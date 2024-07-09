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
    public abstract partial class BaseRepository
    {
        /// <summary>
        /// 
        /// </summary>
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

}
