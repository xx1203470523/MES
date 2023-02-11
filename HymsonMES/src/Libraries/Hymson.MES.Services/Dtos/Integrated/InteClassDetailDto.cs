using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 班次详情表
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public class InteClassDetailDto
    {
        /// <summary>
        /// 唯一标识
        /// <summary
        public long Id { get; set; }

        /// <summary>
        /// 创建人
        /// <summary
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// <summary
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// <summary
        public string? UpdatedBy { get; set; }

        /// <summary>
        ///更新时间
        /// <summary
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        ///备注
        /// <summary>
        public string? Remark { get; set; }

        /// <summary>
        ///班次（字典名称：manu_detail_class_type）
        /// </summary>
        public string DetailClassType { get; set; }

        /// <summary>
        ///项目内容
        /// </summary>
        public string ProjectContent { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        public string EndTime { get; set; }
    }

    /// <summary>
    /// 生产班次详情新增输入对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public record InteClassDetailCreateDto : BaseEntityDto
    {
        /// <summary>
        ///班次（字典名称：manu_detail_class_type）
        /// </summary>
        [Required(ErrorMessage = "班次（字典名称：manu_detail_class_type）不能为空")]
        public string DetailClassType { get; set; }

        /// <summary>
        ///项目内容
        /// </summary>
        [Required(ErrorMessage = "项目内容不能为空")]
        public string ProjectContent { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        [RegularExpression(pattern: @"^(([0-1][0-9])|[0-9]|([2][0-3])):[0-6][0-9]:[0-6][0-9]$", ErrorMessage = "开始时间格式为[12:30:00]")]
        public string StartTime { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        [RegularExpression(pattern: @"^(([0-1][0-9])|[0-9]|([2][0-3])):[0-6][0-9]:[0-6][0-9]$", ErrorMessage = "结束时间格式为[12:30:00]")]
        public string EndTime { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 生产班次详情修改输入对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public record InteClassDetailModifyDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        [Required(ErrorMessage = "主键id不能为空")]
        public long Id { get; set; }

        /// <summary>
        ///班次表id
        /// </summary>
        [Required(ErrorMessage = "班次表id不能为空")]
        public long ClassId { get; set; }

        /// <summary>
        ///班次（字典名称：manu_detail_class_type）
        /// </summary>
        [Required(ErrorMessage = "班次（字典名称：manu_detail_class_type）不能为空")]
        public string DetailClassType { get; set; }

        /// <summary>
        ///项目内容
        /// </summary>
        [Required(ErrorMessage = "项目内容不能为空")]
        public string ProjectContent { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        [RegularExpression(pattern: @"^(([0-1][0-9])|[0-9]|([2][0-3])):[0-6][0-9]:[0-6][0-9]$", ErrorMessage = "开始时间格式为[12:30:00]")]
        public string StartTime { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        [RegularExpression(pattern: @"^(([0-1][0-9])|[0-9]|([2][0-3])):[0-6][0-9]:[0-6][0-9]$", ErrorMessage = "结束时间格式为[12:30:00]")]
        public string EndTime { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Remark { get; set; }

    }

    /// <summary>
    /// 生产班次详情查询对象
    /// </summary>
    public class InteClassDetailQueryDto : PagerInfo
    {
    }
}
