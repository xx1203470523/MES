using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 供应商Dto
    /// </summary>
    public record InteSupplierDto : BaseEntityDto
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 供应商描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

    }
}
