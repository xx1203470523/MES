using Dapper;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process.MaskCode
{
    /// <summary>
    /// 仓储（掩码规则维护）
    /// </summary>
    public partial class ProcMaskCodeRuleRepository : BaseRepository, IProcMaskCodeRuleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcMaskCodeRuleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcMaskCodeRuleEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskCodeId"></param>
        /// <returns></returns>
        public async Task<int> ClearByMaskCodeId(long maskCodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { maskCodeId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskCodeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaskCodeRuleEntity>> GetByMaskCodeIdAsync(long maskCodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaskCodeRuleEntity>(GetByIdSql, new { maskCodeId });
        }
    }

    /// <summary>
    /// 仓储（掩码规则维护）
    /// </summary>
    public partial class ProcMaskCodeRuleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `proc_maskcode_rule`(`Id`, `SiteId`, `MaskCodeId`, `SerialNo`, `Rule`, `MatchWay`, `CreatedBy`, `CreatedOn`) VALUES (@Id, @SiteId, @MaskCodeId, @SerialNo, @Rule, @MatchWay, @CreatedBy, @CreatedOn);";
        const string DeleteSql = "DELETE FROM `proc_maskcode_rule` WHERE `MaskCodeId` = @maskCodeId;";
        const string GetByIdSql = "SELECT `Id`, `SerialNo`, `Rule`, MatchWay, `CreatedBy`, `CreatedOn` FROM `proc_maskcode_rule` WHERE `MaskCodeId` = @MaskCodeId;";
    }
}
