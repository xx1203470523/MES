using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 产品检验参数表，数据实体对象   
    /// qual_parameter_verify_product
    /// @author Czhipu
    /// @date 2023-07-21 11:28:09
    /// </summary>
    public class QualParameterVerifyProductEntity : BaseEntity
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        public long WorkCenterId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
