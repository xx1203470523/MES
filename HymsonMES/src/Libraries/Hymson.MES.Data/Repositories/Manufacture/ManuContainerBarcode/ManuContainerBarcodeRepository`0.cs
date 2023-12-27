using Dapper;

namespace Hymson.MES.Data.Repositories.Manufacture;

public partial class ManuContainerBarcodeRepository
{
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuContainerBarcodeQuery query)
    {
        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }

        if (query.ProductId.HasValue)
        {
            sqlBuilder.Where("ProductId = @ProductId");
        }

        if (query.ContainerId.HasValue)
        {
            sqlBuilder.Where("ContainerId = @ContainerId");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialCode))
        {
            sqlBuilder.Where("MaterialCode = @MaterialCode");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialVersion))
        {
            sqlBuilder.Where("MaterialVersion = @MaterialVersion");
        }

        if (!string.IsNullOrWhiteSpace(query.BarCode))
        {
            sqlBuilder.Where("BarCode = @BarCode");
        }

        if (query.BarCodes != null && query.BarCodes.Any())
        {
            sqlBuilder.Where("BarCode IN @BarCodes");
        }

        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }
}