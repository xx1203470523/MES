using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码关系表 查询参数
    /// </summary>
    public class ManuBarcodeRelationQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 投入条码
        /// </summary>
        public IEnumerable<string> InputBarCodes { get; set; } 

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }
}
