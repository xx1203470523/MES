using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 核心服务层基类
    /// </summary>
    public record BaseBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string User { get; set; } = "";

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();
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
