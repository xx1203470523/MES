using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 获取job关联点关联的作业
    /// </summary>
    public class JobRelationBo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { set; get; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { set; get; }

        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum? LinkPoint { get; set; }
    }
}
