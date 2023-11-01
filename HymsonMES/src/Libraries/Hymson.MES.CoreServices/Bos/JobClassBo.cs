namespace Hymson.MES.CoreServices.Bos
{
    /// <summary>
    /// 
    /// </summary>
    public class JobClassBo
    {
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; } = "";

        /// <summary>
        /// 类命名空间
        /// </summary>
        public string ClassNamespace { get; set; } = "";

        /// <summary>
        /// 类模块
        /// </summary>
        public string ClassModule { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
