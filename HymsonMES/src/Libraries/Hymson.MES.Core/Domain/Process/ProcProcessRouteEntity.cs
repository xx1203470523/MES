using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工艺路线表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcessRouteEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :工艺路线代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述 :工艺路线名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 描述 :类型 
        /// 空值 : true  
        /// </summary>
        public ProcessRouteTypeEnum Type { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述 :是否当前版本 
        /// 空值 : true  
        /// </summary>
        public bool IsCurrentVersion { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    ///根具编码查询工艺路线
    /// </summary>
    public class ProcProcessRoutesByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string?> Codes { get; set; }

    }
}