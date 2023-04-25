using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Report
{
    public class OriginalSummaryReportDto
    {
    }

    public class OriginalSummaryQueryDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 查看类型
        /// </summary>
        public InProductDismantleTypeEnum Type { get; set; }
    }
}
