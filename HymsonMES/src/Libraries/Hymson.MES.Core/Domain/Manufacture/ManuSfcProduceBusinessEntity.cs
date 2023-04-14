using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码业务表，数据实体对象   
    /// manu_sfc_produce_business
    /// @author wangkeming
    /// @date 2023-03-25 04:08:57
    /// </summary>
    public class ManuSfcProduceBusinessEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public long SfcInfoId { get; set; }

       /// <summary>
        /// 业务类型;1、返修业务  2、锁业务
        /// </summary>
        public ManuSfcProduceBusinessType BusinessType { get; set; }

       /// <summary>
        /// 业务内容
        /// </summary>
        public string BusinessContent { get; set; }
    }
}
