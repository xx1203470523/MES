namespace Hymson.MES.CoreServices.Dtos.Common
{
    /// <summary>
    /// 基础信息实体
    /// </summary>
    public class BaseInfoDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 故障现象代码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 故障现象名称
        /// </summary>
        public string Name { get; set; } = "";
    }
}
