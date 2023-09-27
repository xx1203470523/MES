/*
 *creator: Karl
 *
 *describe: 托盘条码关系 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘条码关系仓储
    /// </summary>
    public partial class ManuNgJudgeRepository : BaseRepository, IManuNgJudgeRepository
    {

        public ManuNgJudgeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcNgJudgeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcNgJudgeEntity manuSfcNgJudgeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcNgJudgeEntity);
        }
        #endregion

    }

    public partial class ManuNgJudgeRepository
    {
        #region 

        const string InsertSql = "INSERT INTO `manu_sfc_ng_Judge`(  `Id`, `SiteId`, `SFC`, `StepId`, `NGJudgeType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @StepId, @NGJudgeType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        
        #endregion
    }
}
