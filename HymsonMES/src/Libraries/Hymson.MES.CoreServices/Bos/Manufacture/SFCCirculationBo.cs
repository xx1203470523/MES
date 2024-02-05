using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 返修业务
    /// </summary>
    public class SFCCirculationBo: SingleSFCBo
    {
        /// <summary>
        /// 查看类型
        /// </summary>
        public SFCCirculationReportTypeEnum Type { get; set; }
    }
}
