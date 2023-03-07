using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 保存Dto（班制维护）
    /// </summary>
    public record InteClassSaveDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///班次名称
        /// </summary>
        public string ClassName { get; set; } = "";

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        public int ClassType { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string Remark { get; set; } = "";


        /// <summary>
        /// 
        /// </summary>
        public List<InteClassDetailCreateDto> DetailList { get; set; } = new();
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
        public int ClassType { get; set; }

        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }


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
        public string? ClassName { get; set; }

        /// <summary>
        ///班次类型（字典名称：manu_class_type）
        /// </summary>
        public int? ClassType { get; set; }
    }
}
