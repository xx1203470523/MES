using Dapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment;

public partial class EquEquipmentTheoryRepository : BaseRepository, IEquEquipmentTheoryRepository
{

    public EquEquipmentTheoryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {

    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquEquipmentTheoryEntity>> GetListAsync(EquEquipmentTheoryQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(GetListSql);

        if (query.EquipmentCodes != null)
        {
            sqlBuilder.Where("EquipmentCode IN @EquipmentCodes");
        }

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<EquEquipmentTheoryEntity>(sqlTemplete.RawSql, sqlTemplete.Parameters);
    }

    /// <summary>
    /// 插入产能数
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(EquEquipmentTheoryCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(EquEquipmentTheoryUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, command);
    }

    /// <summary>
    /// 新增或更新
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertOrUpdateAsync(EquEquipmentTheoryUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertOrUpdateSql, command);
    }

    public async Task<EquEquipmentTheoryEntity> GetOneAsync(EquEquipmentTheoryQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(GetOneSql);

        if (query.EquipmentCode != null)
        {
            sqlBuilder.Where("EquipmentCode = @EquipmentCode");
        }

        if (query.EquipmentCodes?.Any() == true)
        {
            sqlBuilder.Where("EquipmentCode IN @EquipmentCodes");
        }

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<EquEquipmentTheoryEntity>(sqlTemplete.RawSql, sqlTemplete.Parameters);
    }
}


public partial class EquEquipmentTheoryRepository
{
    const string GetOneSql = @"SELECT * FROM equ_equipment_theory eet /**where**/ LIMIT 1";
    const string GetListSql = @"SELECT * FROM equ_equipment_theory eet /**where**/";

    const string InsertSql = "INSERT INTO `equ_equipment_theory` (`Id`,`SiteId`,`EquipmentCode`,`TheoryOutputQty`,`OutputQty`,`TheoryOnTime`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`)" +
        " VALUES (@Id,@SiteId,@EquipmentCode,@TheoryOutputQty,@OutputQty,@TheoryOnTime,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted);";

    const string UpdateSql = @"UPDATE mes_master_qingan.equ_equipment_theory
SET  TheoryOutputQty=@TheoryOutputQty, OutputQty=@OutputQty,`TheoryOnTime` = @TheoryOnTime
WHERE EquipmentCode = EquipmentCode ;";

    const string InsertOrUpdateSql = @"INSERT INTO equ_equipment_theory(Id, SiteId, EquipmentCode, TheoryOutputQty, OutputQty, TheoryOnTime, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted)
VALUES(@Id, @SiteId, @EquipmentCode, @TheoryOutputQty, @OutputQty, @TheoryOnTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted)
ON DUPLICATE KEY UPDATE TheoryOutputQty = @TheoryOutputQty,OutputQty = @OutputQty,TheoryOnTime = @TheoryOnTime";


    #region 修改

    const string UpdateByIdSql = "UPDATE `equ_equipment_theory` SET `TheoryOutputQty` = @TheoryOutputQty ,`OutputQty` = @OutputQty ,`TheoryOnTime` = @TheoryOnTime  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion
}