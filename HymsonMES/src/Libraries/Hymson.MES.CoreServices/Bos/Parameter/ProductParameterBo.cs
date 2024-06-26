using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 参数信息
    /// </summary>
    public record ProductParameterBo
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; } = "";

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; } = "";

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; } = HymsonClock.Now();

        /// <summary>
        /// 产品参数组id
        /// </summary>
        public long ParameterGroupId { get; set; } = 0;
    }

}
