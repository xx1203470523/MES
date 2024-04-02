using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 设备投料非生产投料(制胶匀浆)
    /// </summary>
    public record ConsumeInNonProductionEquDto : QknyBaseDto
    {
        /// <summary>
        /// 投料至设备
        /// </summary>
        public string ConsumeEquipmentCode { get; set; } = "";

        /// <summary>
        /// 投料至资源
        /// </summary>
        public string ConsumeResourceCode { get; set; } = "";

        /// <summary>
        /// 消耗列表
        /// </summary>
        public List<ConsumeSfcDto> ConsumeSfcList { get; set; } = new List<ConsumeSfcDto>();
    }   
}
