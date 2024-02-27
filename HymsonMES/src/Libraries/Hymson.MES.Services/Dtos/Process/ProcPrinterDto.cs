using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    public record ProcPrinterDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :打印机名称 
        /// 空值 : false  
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 描述 :打印机IP 
        /// 空值 : false  
        /// </summary>
        public string PrintIp { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }
    public record ProcPrinterUpdateDto:BaseEntityDto 
    {
        /// <summary>
        /// 唯一Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 描述 :打印机名称 
        /// 空值 : false  
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProcPrinterViewDto: PagerInfo
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :打印机名称 
        /// 空值 : false  
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 描述 :打印机IP 
        /// 空值 : false  
        /// </summary>
        public string PrintIp { get; set; }

        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
    public class ProcPrinterPagedQueryDto:PagerInfo
    {
        /// <summary>
        /// 描述 :打印机名称 
        /// 空值 : false  
        /// </summary>
        public string? PrintName { get; set; }

        /// <summary>
        /// 描述 :打印机IP 
        /// 空值 : false  
        /// </summary>
        public string? PrintIp { get; set; }
    }
}

