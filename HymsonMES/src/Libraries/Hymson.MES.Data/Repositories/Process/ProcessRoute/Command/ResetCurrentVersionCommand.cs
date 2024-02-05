using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process.ProcessRoute.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetCurrentVersionCommand : UpdateCommand
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

    }
}
