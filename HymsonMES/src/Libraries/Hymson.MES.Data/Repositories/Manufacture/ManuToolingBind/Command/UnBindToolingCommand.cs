using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuToolingBind.Command
{
    public class UnBindToolingCommand : UpdateCommand
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工装编码
        /// </summary>
        public string ToolingCode { get; set; }
    }
}
