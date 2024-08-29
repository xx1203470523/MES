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
            order by UpdatedOn desc
            and ProductionCode in @SfcList
        ";
    }
}
