using Dapper;
using Hymson.DbConnection.Abstractions;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Stator;
using Hymson.MES.Core.Domain.Mavel.Stator;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
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
    public class PackListRepository : Data.Repositories.BaseRepository, IPackListRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public PackListRepository(IOptions<Data.Options.ConnectionOptions> connectionOptions) : base(connectionOptions)
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

        /// <summary>
        /// 获取定子线组装好的数据
        /// </summary>
        /// <param name="ProductionCode"></param>
        /// <returns></returns>
        public async Task<List<ManuStatorBarcodeEntity>> GetStatorListAsync(string ProductionCode)
        {
            string sql = $@"
                select * from manu_stator_barcode msb 
                where ProductionCode > '{ProductionCode}'
                and ProductionCode  is not null
                order by ProductionCode asc
                limit 0,10
            ";
            using var conn = new MySqlConnection("Data Source=192.168.180.145;port=3371;User ID=sa;Password=qwe123;Database=mes_master_prod;CharSet=utf8;sslmode=none;MaximumPoolSize=1000;");
            var dbList = await conn.QueryAsync<ManuStatorBarcodeEntity>(sql);

            return dbList.ToList();
        }

        /// <summary>
        /// 获取定子线组装好的数据
        /// </summary>
        /// <param name="innerId"></param>
        /// <returns></returns>
        public async Task<StatorOp340> GetStatorOp340Async(string innerId)
        {
            string sql = $@"
                select * from op340 o 
                where id = '{innerId}'
            ";

            using var conn = new MySqlConnection("Data Source=192.168.180.200;port=3306;User ID=root;Password=mavel@2024;Database=mavel;CharSet=utf8;sslmode=none;MaximumPoolSize=200;");
            var dbModel = await conn.QueryFirstOrDefaultAsync<StatorOp340>(sql);

            return dbModel;
        }
    }
}
