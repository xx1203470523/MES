using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过程参数仓储
    /// </summary>
    public partial class ManuProductParameterRepository : BaseRepository, IManuProductParameterRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuProductParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuProductParameterEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuProductParameterEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductParameterRepository
    {
        const string InsertSql = "INSERT INTO `manu_product_parameter`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `parameterId`, `ParamValue`, Timestamp, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @parameterId, @ParamValue, @Timestamp, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_product_parameter`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `parameterId`, `ParamValue`, Timestamp, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @parameterId, @ParamValue, @Timestamp, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    }
}
