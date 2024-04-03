using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductBadRecordBySfcQuery
    {
        /// <summary>
        /// 不合格记录开关;1、开启  2、关闭
        /// </summary>
        public ProductBadRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 拦截工序Id
        /// </summary>
        public long? InterceptOperationId { get; set; }
    }
}
