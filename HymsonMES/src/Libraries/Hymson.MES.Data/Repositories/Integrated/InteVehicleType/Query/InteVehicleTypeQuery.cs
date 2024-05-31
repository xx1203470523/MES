/*
 *creator: Karl
 *
 *describe: 载具类型维护 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具类型维护 查询参数
    /// </summary>
    public class InteVehicleTypeQuery
    {
    }

    /// <summary>
    /// 载具类型维护 编码查询参数
    /// </summary>
    public class InteVehicleTypeCodeQuery
    {
        public string Code { get; set; }

        public long SiteId { get; set; }
    }

    /// <summary>
    /// 载具类型维护 名称查询参数
    /// </summary>
    public class InteVehicleTypeNameQuery
    {
        /// <summary>
        /// 编码
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }

        public long SiteId { get; set; }
    }
}
