using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSFCNode.View
{
    public class ManuSFCNodeView : ManuSFCNodeEntity
    {
        /// <summary>
        /// 父级NodeId
        /// </summary>
        public long ParentNodeId { get; set; }

        /// <summary>
        /// 父级条码
        /// </summary>
        public string ParentNodeSFC { get; set; }
    }
}
