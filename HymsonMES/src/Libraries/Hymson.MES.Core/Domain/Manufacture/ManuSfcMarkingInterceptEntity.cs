using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（Marking拦截记录表）   
    /// manu_sfc_marking_intercept
    /// @author xiaofei
    /// @date 2024-07-24 08:40:52
    /// </summary>
    public class ManuSfcMarkingInterceptEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// Marking信息表Id
        /// </summary>
        public long SfcMarkingId { get; set; }

        /// <summary>
        /// 拦截工序
        /// </summary>
        public long InterceptProcedureId { get; set; }

        /// <summary>
        /// 拦截设备
        /// </summary>
        public long InterceptEquipmentId { get; set; }

        /// <summary>
        /// 拦截资源
        /// </summary>
        public long InterceptResourceId { get; set; }

        /// <summary>
        /// 拦截时间
        /// </summary>
        public DateTime InterceptOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        
    }
}
