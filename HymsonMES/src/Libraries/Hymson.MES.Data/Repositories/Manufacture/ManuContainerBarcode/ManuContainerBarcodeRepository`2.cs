using Dapper;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 方法
/// </summary>
public partial class ManuContainerBarcodeRepository
{
    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertReAsync(ManuContainerBarcodeEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertReSql, entity);
    }

    /// <summary>
    /// 刷新容器条码状态
    /// </summary>
    /// <returns></returns>
    public async Task<int> RefreshStatusAsync(RefreshStatusCommand command)
    {
        using var conn = GetMESDbConnection();

        return await conn.ExecuteAsync(RefreshStatusSql, command);
    }

    /// <summary>
    /// Qty增量
    /// </summary>
    /// <returns></returns>
    public async Task<int> IncrementQtyAsync(IncrementQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(IncrementQtySql, command);
    }

    /// <summary>
    /// 刷新Qty数量
    /// </summary>
    /// <returns></returns>
    public async Task<int> RefreshQtyAsync(RefreshQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(RefreshQtySql, command);
    }

    /// <summary>
    /// Qty增量
    /// </summary>
    /// <returns></returns>
    public async Task<int> ClearQtyAsync(ClearQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(ClearQtySql, command);
    }

    /// <summary>
    /// 关闭容器
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> ChangeContainerStatusAsync(CloseContainerCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(ChangeContainerStatusSql, command);
    }
}

/// <summary>
/// SQL
/// </summary>
public partial class ManuContainerBarcodeRepository
{
    /// <summary>
    /// 新增
    /// </summary>
    const string InsertReSql = "INSERT INTO `manu_container_barcode` (`Id`,`SiteId`,`BarCode`,`ContainerId`,`Status`,`Qty`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (@Id,@SiteId,@BarCode,@ContainerId,@Status,@Qty,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted);";


#if DM
    /// <summary>
    /// Qty增量
    /// </summary>
    const string IncrementQtySql = "UPDATE manu_container_barcode SET Qty = Qty + CAST(@IncrementValue AS DECIMAL) WHERE Id = @Id AND (Qty + CAST(@IncrementValue AS DECIMAL)) <= @MaxValue";
#else
    /// <summary>
    /// Qty增量
    /// </summary>
    const string IncrementQtySql = "UPDATE manu_container_barcode SET Qty = Qty + @IncrementValue WHERE Id = @Id AND Qty + @IncrementValue <= @MaxValue";
#endif


    /// <summary>
    /// 刷新状态
    /// </summary>
    const string RefreshStatusSql = "UPDATE manu_container_barcode SET `Status` = 2 WHERE Id = @Id AND Qty >= @MaxValue;";

    /// <summary>
    /// 刷新Qty数量
    /// </summary>
    const string RefreshQtySql = "UPDATE manu_container_barcode SET Qty = (SELECT COUNT(1) FROM manu_container_pack WHERE ContainerBarCodeId = @Id) WHERE Id = @Id";

    /// <summary>
    /// 清空Qty数量
    /// </summary>
    const string ClearQtySql = "UPDATE manu_container_barcode SET Qty = 0 WHERE Id = @Id;";

    /// <summary>
    /// 改变容器条码状态
    /// </summary>
    const string ChangeContainerStatusSql = "UPDATE manu_container_barcode SET `Status` = @Status WHERE Id = @Id AND Status = @StatusCondition";
}
