using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 产品参数上传
    /// </summary>
    public record ProductParamDto : QknyBaseDto
    {
        /// <summary>
        /// 容器条码
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 出站产品条码列表
        /// </summary>
        public List<ProductParamSfcDto> SfcList { get; set; } = new List<ProductParamSfcDto>();
    }

    /// <summary>
    /// 条码参数
    /// </summary>
    public record ProductParamSfcDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }

    /// <summary>
    /// 多个条码参数相同
    /// </summary>
    public record ProductParamSameMultSfcDto : QknyBaseDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList = new List<string>();

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }
}
