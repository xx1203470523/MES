namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 核心服务层基类
    /// </summary>
    public class CoreBaseBo
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserName { get; set; } = "";
    }
}
