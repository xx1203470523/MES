using Dapper;

namespace Hymson.MES.Data.Repositories.Manufacture;

public partial class ManuContainerPackRepository
{
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuContainerPackQuery query)
    {
        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }

        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }

        if (query.ResourceId.HasValue)
        {
            sqlBuilder.Where("ResourceId = @ResourceId");
        }        

        if (query.ContainerBarCodeId.HasValue)
        {
            sqlBuilder.Where("ContainerBarCodeId = @ContainerBarCodeId");
        }

        if (query.ContainerBarCodeIds != null && query.ContainerBarCodeIds.Any())
        {
            sqlBuilder.Where("ContainerBarCodeId IN @ContainerBarCodeIds");
        }

        if (!string.IsNullOrWhiteSpace(query.LadeBarCode))
        {
            sqlBuilder.Where("LadeBarCode = @LadeBarCode");
        }

        if (query.LadeBarCodes != null && query.LadeBarCodes.Any())
        {
            sqlBuilder.Where("LadeBarCode IN @LadeBarCodes");
        }

        if (query.OutermostContainerBarCodeId.HasValue)
        {
            sqlBuilder.Where("OutermostContainerBarCodeId = @OutermostContainerBarCodeId");
        }

        if(query.OutermostContainerBarCodeIds != null && query.OutermostContainerBarCodeIds.Any())
        {
            sqlBuilder.Where("OutermostContainerBarCodeId IN @OutermostContainerBarCodeIds");
        }

        if (query.DeepMax.HasValue)
        {
            sqlBuilder.Where("Deep <= @DeepMax");
        }

        if (query.DeepMin.HasValue)
        {
            sqlBuilder.Where("Deep > @DeepMin");
        }

        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }
}
