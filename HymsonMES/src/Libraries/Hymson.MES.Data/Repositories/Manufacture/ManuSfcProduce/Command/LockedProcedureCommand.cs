using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 在制品锁定实体
    /// </summary>
    public  class LockedProcedureCommand
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public  IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }
    }
}
