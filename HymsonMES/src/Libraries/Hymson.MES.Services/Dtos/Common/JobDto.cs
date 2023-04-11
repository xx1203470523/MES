using Hymson.MES.Services.BOs.Manufacture;

namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 作业Dto
    /// </summary>
    public class JobDto: ManufactureBO
    {
        /// <summary>
        /// 面板ID
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public long FacePlateButtonId { get; set; }

    }

}
