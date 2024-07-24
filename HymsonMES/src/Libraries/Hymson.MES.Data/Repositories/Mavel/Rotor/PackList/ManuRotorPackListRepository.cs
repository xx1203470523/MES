using Dapper;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Rotor.PackList
{
    /// <summary>
    /// 转子包装仓储
    /// </summary>
    public partial class ManuRotorPackListRepository : BaseRepository, IManuRotorPackListRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuRotorPackListRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuRotorPackListEntity> list)
        {
            if (list == null || list.Count == 0)
            {
                return 0;
            }

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, list);
        }
    }

    public partial class ManuRotorPackListRepository
    {
        const string InsertsSql = $@"
            INSERT INTO mes_master_test.manu_rotor_pack_list
            (Id, BoxCode, ProductCode, ProductNo, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId)
            VALUES(@Id, @BoxCode, @ProductCode, @ProductNo, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId);
        ";
    }
}
