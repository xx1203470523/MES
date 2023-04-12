using Hymson.MES.Services.Bos.Manufacture;

namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 作业Dto
    /// </summary>
    public class JobDto: ManufactureBo
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
