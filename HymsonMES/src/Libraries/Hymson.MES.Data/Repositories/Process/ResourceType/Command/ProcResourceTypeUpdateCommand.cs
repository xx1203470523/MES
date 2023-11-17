using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.ResourceType
{
    public class ProcResourceTypeUpdateCommand:BaseEntity
    {
        public new long Id { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联的资源Id
        /// </summary>
        public IEnumerable<string> ResourceIds { get; set; }
    }
}
