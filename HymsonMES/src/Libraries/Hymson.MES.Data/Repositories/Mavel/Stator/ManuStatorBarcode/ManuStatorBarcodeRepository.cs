using Dapper;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Domain.Mavel.Stator;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Mavel.Rotor;
using Hymson.MES.Data.Repositories.Mavel.Stator.ManuStatorBarcode.Query;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Stator.ManuStatorBarcode
{
    /// <summary>
    /// 转子条码查询
    /// </summary>
    public partial class ManuStatorBarcodeRepository : BaseRepository, IManuStatorBarcodeRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuStatorBarcodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 查询总成码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuStatorBarcodeEntity>> GetListBySfcsAsync(StatorSfcQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuStatorBarcodeEntity>(SfcQuerySql, query);
        }

        /// <summary>
        /// 根据水位查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuStatorBarcodeEntity>> GetListByWatersAsync(StatorSfcWaterQuery query)
        {
            string sql = $@"
                select * 
                from manu_stator_barcode  
                where ProductionCode is not null
                and UpdatedOn > '{query.StartWaterMarkTime.ToString("yyyy-MM-dd HH:mm:ss")}'
                order by UpdatedOn desc
                limit 0,{query.Rows}
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuStatorBarcodeEntity>(sql, query);
        }
    }

    /// <summary>
    /// SQL
    /// </summary>
    public partial class ManuStatorBarcodeRepository
    {
        const string SfcQuerySql = $@"
            select * 
            from manu_stator_barcode  
            where SiteId  = @SiteId
            and ProductionCode in @SfcList
            order by UpdatedOn desc
        ";
    }
}
