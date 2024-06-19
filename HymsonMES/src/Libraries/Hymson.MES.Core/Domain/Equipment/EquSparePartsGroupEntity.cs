using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（备件类型）   
    /// equ_sparepart_type
    /// @author kongaomeng
    /// @date 2023-12-15 10:56:56
    /// </summary>
    public class EquSparePartsGroupEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";

       
    }
}
