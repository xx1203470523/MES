using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Text;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    public partial class ManuFeedingRepository
    {
        /// <summary>
        /// 获取上料信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetOneAsync(ManuFeedingQuery query)
        {
            var sqlBuilder = new SqlBuilder();

            var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

            if (!string.IsNullOrWhiteSpace(query.BarCode))
            {
                sqlBuilder.Where("BarCode = @BarCode");
            }

            sqlBuilder.Where("IsDeleted = 0");

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();

            return await conn.QueryFirstOrDefaultAsync<ManuFeedingEntity>(templateData.RawSql, templateData.Parameters);
        }
    }

    public partial class ManuFeedingRepository
    {
        const string GetOneSqlTemplate = "SELECT * FROM manu_feeding /**where**/ LIMIT 1;";
    }
}
