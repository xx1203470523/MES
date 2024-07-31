using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 条码追溯工序DTO 视图模型
    /// </summary>
    public record ProcedureSourceDto:BaseEntityDto
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 工单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }




    }
}
