using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 生产班次新增输入对象
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
        public string SiteCode { get; set; }
    }

    /// <summary>
    /// 生产班次新增输入对象
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
    /// 生产班次新增输入对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public record nteClassCreateDto : BaseEntityDto
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

        public List<AddInteClassDetailDto> DetailList { get; set; }
    }

    /// <summary>
    /// 生产班次修改输入对象
    /// @author wangkeming
    /// @date 2022-12-26
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
        public List<UpdateInteClassDetailDto> DetailList { get; set; }
    }

    /// <summary>
    /// 生产班次查询对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public class InteClassPagedQueryDto : PagerInfo
    {
        /// <summary>
        ///班次名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        public string ClassType { get; set; }
    }
}
