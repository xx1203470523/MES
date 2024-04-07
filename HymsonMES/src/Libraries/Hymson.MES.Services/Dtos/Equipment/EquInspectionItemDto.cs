using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备点检保养项目新增/更新Dto
    /// </summary>
    public record EquInspectionItemSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string OperationMethod { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";
    }

    /// <summary>
    /// 设备点检保养项目Dto
    /// </summary>
    public record EquInspectionItemDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作方法
        /// </summary>
        public string OperationMethod { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>
    /// 设备点检保养项目分页Dto
    /// </summary>
    public class EquInspectionItemPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }
    }

}
