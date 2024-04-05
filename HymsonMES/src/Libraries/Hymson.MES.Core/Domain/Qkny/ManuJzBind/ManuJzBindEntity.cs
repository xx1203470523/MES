using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.ManuJzBind
{
    /// <summary>
    /// 数据实体（极组绑定）   
    /// manu_jz_bind
    /// @author Yxx
    /// @date 2024-04-04 04:41:24
    /// </summary>
    public class ManuJzBindEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 极组条码1
        /// </summary>
        public string JzSfc1 { get; set; }

        /// <summary>
        /// 极组条码2
        /// </summary>
        public string JzSfc2 { get; set; }

        /// <summary>
        /// 电芯码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 绑定类型
        /// </summary>
        public string BindType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
