using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 补液数据上传
    /// </summary>
    public record FillingDataDto : QknyBaseDto
    {
        /// <summary>  
        /// 电芯进入一次注液时间  
        /// </summary>  
        public DateTime InTime { get; set; }

        /// <summary>  
        /// 电芯离开一次注液时间  
        /// </summary>  
        public DateTime OutTime { get; set; }

        /// <summary>  
        /// 注液前重量  
        /// </summary>  
        public decimal BeforeWeight { get; set; }

        /// <summary>  
        /// 注液后重量  
        /// </summary>  
        public decimal AfterWeight { get; set; }

        /// <summary>  
        /// 自动注液量  
        /// </summary>  
        public decimal ElWeight { get; set; }

        /// <summary>  
        /// 自动补液量  
        /// </summary>  
        public decimal AddEl { get; set; }

        /// <summary>  
        /// 自动注液总量  
        /// </summary>  
        public decimal TotalEl { get; set; }

        /// <summary>  
        /// 手工补液量  
        /// </summary>  
        public decimal ManualEl { get; set; }

        /// <summary>  
        /// 最终注液量  
        /// </summary>  
        public decimal FinalEl { get; set; }

        /// <summary>  
        /// 补液结果  
        /// </summary>  
        public bool IsOk { get; set; }

        /// <summary>  
        /// 电芯条码  
        /// </summary>  
        public string Sfc { get; set; } = "";
    }
}
