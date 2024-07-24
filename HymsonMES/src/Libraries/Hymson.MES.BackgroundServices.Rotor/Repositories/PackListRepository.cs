using Dapper;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 装箱仓储
    /// </summary>
    public class PackListRepository : BaseRepository, IPackListRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public PackListRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<PackListDto>> GetList(string sql)
        {
            using var conn = GetRotorDbConnection();
            var dbList = await conn.QueryAsync<PackListDto>(sql);

            return dbList.ToList();
        }
    }
}
