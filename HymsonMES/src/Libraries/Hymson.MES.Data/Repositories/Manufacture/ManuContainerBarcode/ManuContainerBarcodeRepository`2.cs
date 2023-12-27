using Dapper;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// ����
/// </summary>
public partial class ManuContainerBarcodeRepository
{
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertReAsync(ManuContainerBarcodeEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertReSql, entity);
    }

    /// <summary>
    /// ˢ����������״̬
    /// </summary>
    /// <returns></returns>
    public async Task<int> RefreshStatusAsync(RefreshStatusCommand command)
    {
        using var conn = GetMESDbConnection();

        return await conn.ExecuteAsync(RefreshStatusSql, command);
    }

    /// <summary>
    /// Qty����
    /// </summary>
    /// <returns></returns>
    public async Task<int> IncrementQtyAsync(IncrementQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(IncrementQtySql, command);
    }

    /// <summary>
    /// ˢ��Qty����
    /// </summary>
    /// <returns></returns>
    public async Task<int> RefreshQtyAsync(RefreshQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(RefreshQtySql, command);
    }

    /// <summary>
    /// Qty����
    /// </summary>
    /// <returns></returns>
    public async Task<int> ClearQtyAsync(ClearQtyCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(ClearQtySql, command);
    }

    /// <summary>
    /// �ر�����
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
    /// ����
    /// </summary>
    const string InsertReSql = "INSERT INTO `manu_container_barcode` (`Id`,`SiteId`,`BarCode`,`ContainerId`,`Status`,`Qty`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (@Id,@SiteId,@BarCode,@ContainerId,@Status,@Qty,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted);";

    /// <summary>
    /// Qty����
    /// </summary>
    const string IncrementQtySql = "UPDATE manu_container_barcode SET Qty = Qty + @IncrementValue WHERE Id = @Id AND Qty + @IncrementValue <= @MaxValue";

    /// <summary>
    /// ˢ��״̬
    /// </summary>
    const string RefreshStatusSql = "UPDATE manu_container_barcode SET `Status` = 2 WHERE Id = @Id AND Qty >= @MaxValue;";

    /// <summary>
    /// ˢ��Qty����
    /// </summary>
    const string RefreshQtySql = "UPDATE manu_container_barcode SET Qty = (SELECT COUNT(1) FROM manu_container_pack WHERE ContainerBarCodeId = @Id) WHERE Id = @Id";

    /// <summary>
    /// ���Qty����
    /// </summary>
    const string ClearQtySql = "UPDATE manu_container_barcode SET Qty = 0 WHERE Id = @Id;";

    /// <summary>
    /// �ı���������״̬
    /// </summary>
    const string ChangeContainerStatusSql = "UPDATE manu_container_barcode SET `Status` = @Status WHERE Id = @Id AND Status = @StatusCondition";
}
