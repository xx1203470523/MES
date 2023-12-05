namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 核心服务层基类
    /// </summary>
    public record BaseBo
    {
        // 暂时没想好放什么
    }

    /// <summary>
    /// 核心服务层基类
    /// </summary>
    public record QueryByIdBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 目标ID
        /// </summary>
        public long QueryId { get; set; }
    }
}
