using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 更新
    /// </summary>
    public record EquMaintenanceItemUpdateDto : EquMaintenanceItemSaveDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        ///// <summary>
        // /// 创建时间
        // /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 设备保养项目新增/更新Dto
    /// </summary>
    public record EquMaintenanceItemSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 数值类型;文本/数值
        /// </summary>
        public DataTypeEnum? DataType { get; set; } = DataTypeEnum.Text;

        /// <summary>
        /// 点检方式
        /// </summary>
        public EquSpotcheckItemMethodEnum? CheckType { get; set; }

        /// <summary>
        /// 作业方法
        /// </summary>
        public string? CheckMethod { get; set; }

        /// <summary>
        /// 单位ID;inte_unit表的Id
        /// </summary>
        public long? UnitId { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string? OperationContent { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        public string? Components { get; set; }

        /// <summary>
        /// 描述;项目描述
        /// </summary>
        public string? Remark { get; set; }


    }

    /// <summary>
    /// 设备保养项目Dto
    /// </summary>
    public record EquMaintenanceItemDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点检项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 点检项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 数值类型;文本/数值
        /// </summary>
        public DataTypeEnum DataType { get; set; } = DataTypeEnum.Text;

        /// <summary>
        /// 点检方式
        /// </summary>
        public EquSpotcheckItemMethodEnum? CheckType { get; set; }

        /// <summary>
        /// 作业方法
        /// </summary>
        public string CheckMethod { get; set; }

        /// <summary>
        /// 单位ID;inte_unit表的Id
        /// </summary>
        public long? UnitId { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationContent { get; set; }

        /// <summary>
        /// 部件
        /// </summary>
        public string Components { get; set; }

        /// <summary>
        /// 描述;项目描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 设备保养项目分页Dto
    /// </summary>
    public class EquMaintenanceItemPagedQueryDto : PagerInfo
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
