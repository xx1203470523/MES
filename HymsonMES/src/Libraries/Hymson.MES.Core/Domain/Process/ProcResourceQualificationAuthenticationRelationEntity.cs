using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（资源资质认证（物理删除））   
    /// proc_resource_qualification_authentication_relation
    /// @author zhaoqing
    /// @date 2024-06-18 06:00:25
    /// </summary>
    public class ProcResourceQualificationAuthenticationRelationEntity : BaseEntity
    {
        /// <summary>
        /// proc_resource资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资质Id inte_qualification_authentication的Id
        /// </summary>
        public long QualificationAuthenticationId { get; set; }

        /// <summary>
        /// 是否启用  0 未启用 1启用
        /// </summary>
        public bool IsEnable { get; set; }

        
    }
}
