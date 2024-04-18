using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（工作中心关联信息）
    /// @author Czhipu
    /// @date 2022-10-09
    /// </summary>
    public class InteWorkCenterLink : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 序号( 程序生成) 
        /// </summary>
        public long SerialNo { get; set; }
        /// <summary>
        /// 描述 :所属工作中心ID 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterId { get; set; }
        /// <summary>
        /// 描述 :工作中心ID  
        /// 空值 : false  
        /// </summary>
        public long SubWorkCenterId { get; set; }
        /// <summary>
        /// 描述 :资源ID 
        /// 空值 : false  
        /// </summary>
        public long SubResourceId { get; set; }
    }
}
