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
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }

}
