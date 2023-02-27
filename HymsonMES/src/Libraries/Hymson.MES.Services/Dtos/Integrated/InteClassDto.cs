using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 新增Dto（班制维护）
    /// </summary>
    public record InteClassCreateDto : BaseEntityDto
    {
        /// <summary>
        ///班次名称
        /// </summary>
        [Required(ErrorMessage = "班次名称不能为空")]
        public string ClassName { get; set; }

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        [Required(ErrorMessage = "班次类型不能为空")]
        public string ClassType { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Remark { get; set; }

        public List<InteClassDetailCreateDto> DetailList { get; set; }
    }

    /// <summary>
    /// 更新Dto（班制维护）
    /// </summary>
    public record InteClassModifyDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        [Required(ErrorMessage = "主键id不能为空")]
        public long Id { get; set; }

        /// <summary>
        ///班次名称
        /// </summary>
        [Required(ErrorMessage = "班次名称不能为空")]
        public string ClassName { get; set; }

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        [Required(ErrorMessage = "班次类型（字典名称：manu_class_type）不能为空")]
        public string ClassType { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<InteClassDetailModifyDto> DetailList { get; set; }
    }

    /// <summary>
    /// 删除Dto（班制维护）
    /// </summary>
    public record InteClassDeleteDto
    {
        /// <summary>
        /// 集合（主键）
        /// </summary>
        public long[] Ids { get; set; }
    }

    /// <summary>
    /// 班制维护新增输入对象
    /// </summary>
    public record InteClassDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 描述 :班次名称 
        /// 空值 : false  
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 描述 :班次类型（字典名称：manu_class_type） 
        /// 空值 : false  
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 班制维护新增输入对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public class InteClassWithDetailDto
    {
        /// <summary>
        /// 班制信息
        /// </summary>
        public InteClassDto ClassInfo { get; set; }

        /// <summary>
        /// 班制详细信息
        /// </summary>
        public List<InteClassDetailDto> DetailList { get; set; } = new();
    }

    /// <summary>
    /// 班制维护查询对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public class InteClassPagedQueryDto : PagerInfo
    {
        /// <summary>
        ///班次名称
        /// </summary>
        public string ClassName { get; set; } = "";

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        public int ClassType { get; set; }
    }
}
