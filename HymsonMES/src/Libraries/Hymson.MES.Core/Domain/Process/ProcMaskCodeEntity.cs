using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 掩码表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcMaskCodeEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :掩码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述 :掩码名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

         /// <summary>
        /// 描述 :掩码 
        /// 空值 : false  
        /// </summary>
        public string MaskCode { get; set; }
    }

    /// <summary>
    ///根具编码查询掩码
    /// </summary>
    public class ProcMaskCodesByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string?> Codes { get; set; }
    }
}