using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 产出米数上报
    /// </summary>
    public record OutboundMetersReportDto : QknyBaseDto
    {
        /// <summary>  
        /// 产品条码
        /// </summary>  
        public string Sfc { get; set; } = "";

        /// <summary>  
        /// 生产总数  
        /// </summary>  
        public decimal TotalQty { get; set; }

        /// <summary>  
        /// 合格数  
        /// </summary>  
        public decimal OkQty { get; set; }

        /// <summary>  
        /// 不合格数  
        /// </summary>  
        public decimal NgQty { get; set; }

        /// <summary>  
        /// 分切数量  
        /// </summary>  
        public int SplitQty { get; set; }

        /// <summary>
        /// 产出类型
        /// 1制胶 2匀浆 3涂布 4辊分 5模切
        /// </summary>
        public string OutputType { get; set; } = string.Empty;

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();

        /// <summary>
        /// NG信息
        /// </summary>
        public List<NgInfoListDto> NgList { get; set; } = new List<NgInfoListDto>();
    }

    /// <summary>
    /// NG信息
    /// </summary>
    public class NgInfoListDto
    {
        /// <summary>
        /// ng代码
        /// </summary>
        public string NgCode { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }
    }
}
