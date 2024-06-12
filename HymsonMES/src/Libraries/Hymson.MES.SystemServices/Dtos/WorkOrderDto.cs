namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// bom替代料
    /// </summary>
    public class BomReplaceMaterialDto
    {
        /// <summary>
        /// 替代料编码
        /// </summary>
        public string AlternativeCode { get; set; }

        /// <summary>
        /// 替代料版本
        /// </summary>
        public string AlternativeVersion { get; set; }

        /// <summary>
        /// 替代料用量
        /// </summary>
        public decimal AlternativeDosage { get; set; }

        /// <summary>
        /// 替代料损耗
        /// </summary>
        public decimal AlternativeLoss { get; set; }
    }
}
