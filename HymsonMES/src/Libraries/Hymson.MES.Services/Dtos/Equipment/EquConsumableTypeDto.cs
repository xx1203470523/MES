using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// Dto（工装类型）
    /// </summary>
    public record EquConsumableTypeDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string ConsumableTypeCode { get; set; } = "";

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string ConsumableTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }

    /// <summary>
    /// 新增Dto（工装类型）
    /// </summary>
    public record EquConsumableTypeCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string ConsumableTypeCode { get; set; } = "";

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string ConsumableTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 工装ID集合
        /// </summary>
        public IEnumerable<long> ConsumableIDs { get; set; }
    }

    /// <summary>
    /// 更新Dto（工装类型）
    /// </summary>
    public record EquConsumableTypeModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string ConsumableTypeCode { get; set; } = "";

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string ConsumableTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 备件ID集合
        /// </summary>
        public IEnumerable<long> ConsumableIDs { get; set; }
    }

    /// <summary>
    /// 删除Dto（工装类型）
    /// </summary>
    public record EquConsumableTypeDeleteDto
    {
        /// <summary>
        /// 集合（主键）
        /// </summary>
        public long[] Ids { get; set; }
    }


    /// <summary>
    /// 分页Dto（工装类型）
    /// </summary>
    public class EquConsumableTypePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string? ConsumableTypeCode { get; set; }

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string? ConsumableTypeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
