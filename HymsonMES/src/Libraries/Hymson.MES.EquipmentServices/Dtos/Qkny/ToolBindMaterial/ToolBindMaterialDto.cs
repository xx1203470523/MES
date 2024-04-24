namespace Hymson.MES.EquipmentServices.Dtos.Qkny.ToolBindMaterial
{
    /// <summary>
    /// 工装条码绑定
    /// </summary>
    public record ToolBindMaterialDto : QknyBaseDto
    {
        /// <summary>
        /// 工装
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 绑定的条码列表
        /// </summary>
        public List<string> ContainerSfcList { get; set; } = new List<string>();
    }
}
