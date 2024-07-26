using Dapper;
using Google.Protobuf.WellKnownTypes;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Mavel.Rotor.ManuRotorSfc.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Rotor
{
    /// <summary>
    /// 转子条码
    /// </summary>
    public partial class ManuRotorSfcRepository : BaseRepository, IManuRotorSfcRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuRotorSfcRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuRotorSfcEntity> entityList)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entityList);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="eneity"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuRotorSfcEntity> eneityList)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, eneityList);
        }

        /// <summary>
        /// 水位查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRotorSfcEntity>> GetListAsync(EntityByWaterMarkTimeQuery query)
        {
            string rowSql = string.Empty;
            if (query.Rows != 0)
            {
                rowSql = "LIMIT @Rows";
            }

            string sql = $@"
                select * 
                from manu_rotor_sfc
                where UpdatedOn > '{query.StartWaterMarkTime.ToString("yyyy-MM-dd HH:mm:ss")}'
                and IsFinish = 1 
                order by UpdatedOn asc
                {rowSql};
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRotorSfcEntity>(sql, query);
        }

        /// <summary>
        /// 根据轴码获取信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuRotorSfcEntity>> GetListByZSfcsAsync(ZSfcQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuRotorSfcEntity>(ZSfcQuerySql, query);
        }

    }

    public partial class ManuRotorSfcRepository
    {
        const string InsertSql = $@"
        INSERT INTO `manu_rotor_sfc`
        (`Id`, `Sfc`, `TxSfc`, `ZSfc`, `WorkOrderId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `IsFinish`, `TxSfcMaterialCode`, `ZSfcMaterialCode`, `SfcMaterialCode`)
        VALUES(@Id, @Sfc, @TxSfc, @ZSfc, @WorkOrderId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @IsFinish,@TxSfcMaterialCode, @ZSfcMaterialCode, @SfcMaterialCode)
        ";

        const string UpdateSql = $@"
            update manu_rotor_sfc
            set sfc = @sfc,updatedBy = @updatedBy,updatedOn = @updatedOn,IsFinish=@IsFinish,ZSfcMaterialCode=@ZSfcMaterialCode
            where zsfc = @zsfc;
        ";

        const string ZSfcQuerySql = $@"
            select * 
            from manu_rotor_sfc  
            where SiteId  = @SiteId
            and ZSfc in @SfcList
        ";
    }
}
