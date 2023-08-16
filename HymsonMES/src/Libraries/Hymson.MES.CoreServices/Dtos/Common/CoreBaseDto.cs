namespace Hymson.MES.CoreServices.Dtos.Common
{
    /// <summary>
    /// Core DTO 基类
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public class CoreBaseDto
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
