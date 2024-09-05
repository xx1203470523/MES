using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（定子装箱记录表）   
    /// manu_stator_pack_list
    /// @author Yxx
    /// @date 2024-09-04 11:54:20
    /// </summary>
    public class ManuStatorPackListEntity : BaseEntity
    {
        /// <summary>
        /// 箱体码
        /// </summary>
        public string BoxCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 品质状态
        /// </summary>
        public string QualStatus { get; set; }

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int BoxNum { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
