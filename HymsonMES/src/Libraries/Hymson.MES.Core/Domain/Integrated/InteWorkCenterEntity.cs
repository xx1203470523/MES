using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 工作中心表数据实体对象
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :站点Id 
        /// 空值 : true  
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :工作中心代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述 :工作中心名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :类型(工厂/车间/产线) 
        /// 空值 : false  
        /// </summary>
        public WorkCenterTypeEnum Type { get; set; }

        /// <summary>
        /// 描述 :数据来源 
        /// 空值 : false  
        /// </summary>
        public WorkCenterSourceEnum Source { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 描述 :是否混线 
        /// 空值 : false  
        /// </summary>
        public bool? IsMixLine { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 线体编码，条码生成使用
        /// </summary>
        public string? LineCoding { get; set; }
    }
}