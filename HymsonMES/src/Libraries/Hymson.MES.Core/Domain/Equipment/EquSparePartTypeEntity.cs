using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体对象（备件类型）  
    /// equ_sparepart_type
    /// @author 陈志谱
    /// @date 2023-02-11 04:10:42
    /// </summary>
    public class EquSparePartTypeEntity : BaseEntity
    {
        /// <summary>
        /// 备件类型编码
        /// </summary>
        public string SparePartTypeCode { get; set; } = "";

       /// <summary>
        /// 备件类型名称
        /// </summary>
        public string SparePartTypeName { get; set; } = "";

        /// <summary>
        /// 备件/工装
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点代码
        /// </summary>
        public string SiteCode { get; set; }
       
    }
}
