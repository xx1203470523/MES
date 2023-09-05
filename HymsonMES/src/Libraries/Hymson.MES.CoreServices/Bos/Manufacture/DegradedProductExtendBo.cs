using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class DegradedProductExtendBo : CoreBaseBo
    {
        /// <summary>
        /// 键值对
        /// </summary>
        public List<DegradedProductExtendKeyValueBo> KeyValues { get; set; } = new();

    }

    /// <summary>
    /// 
    /// </summary>
    public class DegradedProductExtendKeyValueBo
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; } = "";

        /// <summary>
        /// 生产条码
        /// </summary>
        public string SFC { get; set; } = "";

    }


}
