using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 生产班次详情，数据实体对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public class InteClassDetail : BaseEntity
    {
        /// <summary>
        /// 描述 :班次表id 
        /// 空值 : false  
        /// </summary>
        public long ClassId { get; set; }

        /// <summary>
        /// 描述 :班次（字典名称：manu_detail_class_type） 
        /// 空值 : false  
        /// </summary>
        public string DetailClassType { get; set; }

        /// <summary>
        /// 描述 :项目内容 
        /// 空值 : false  
        /// </summary>
        public string ProjectContent { get; set; }

        /// <summary>
        /// 描述 :开始时间 
        /// 空值 : false  
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 描述 :结束时间 
        /// 空值 : false  
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
    }
}
