using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class ClearResourceTypeIdsCommand : UpdateCommand
    {
        /// <summary>
        /// id列表
        /// </summary>
        public long[] ResourceTypeIds { get; set; }
    }
}
