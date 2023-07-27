/*
 *creator: Karl
 *
 *describe: 载具注册表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具注册表 查询参数
    /// </summary>
    public class InteVehicleQuery
    {
    }

    public class InteVehicleCodeQuery
    {
        public string Code { get; set; }

        public long SiteId { get; set; }
    }

    public class InteVehicleVehicleTypeIdsQuery
    {
        public long[] VehicleTypeIds { get; set; }

        public long SiteId { get; set; }
    }
}
