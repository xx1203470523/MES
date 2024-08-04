using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcMarkingBo : CoreBaseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ConsumeSFCs { get; set; }
    }

    /// <summary>
    /// Marking拦截Bo
    /// </summary>
    public class MarkingInterceptBo : CoreBaseBo
    {
        /// <summary>
        /// 进站/上料工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 拦截设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 拦截资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 进站/上料条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }
}
