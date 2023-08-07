using Hymson.MES.EquipmentServices.Dtos.OutBound;

namespace Hymson.MES.EquipmentServicesTests.Dtos
{
    public class ProcessRouteSfcDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 是否已经出站Ng
        /// </summary>
        public bool IsNg { get; set; }

        /// <summary>
        /// NG所在工序
        /// </summary>
        public long? NgProcProcedure { get; set; }

        /// <summary>
        /// 出站NgList
        /// </summary>
        public List<OutBoundNG>? NgList { get; set; }

        /// <summary>
        /// 出站参数
        /// </summary>
        public List<OutBoundParam>? OutBoundParam { get; set; }
    }
}
