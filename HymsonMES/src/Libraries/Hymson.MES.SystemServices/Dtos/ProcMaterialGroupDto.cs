using Hymson.Infrastructure;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 物料组dto类
    /// </summary>
    public record ProcMaterialGroupDto: BaseEntityDto
    {
        /// <summary>
        /// 物料组编码
        /// </summary>
        public string MaterialGroupCode { get; set; }

        /// <summary>
        /// 物料组名称
        /// </summary>
        public string MaterialGroupName { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string? Description { get; set; }
    }
}
