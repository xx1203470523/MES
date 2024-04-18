using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

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
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        ///班次（字典名称：manu_detail_class_type）
        /// </summary>
        public DetailClassTypeEnum DetailClassType { get; set; }

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
    /// 班制维护详情新增输入对象
    /// @author wangkeming
    /// @date 2022-12-26
    /// </summary>
    public record InteClassDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        ///项目内容
        /// </summary>
        public long? SerialNo { get; set; }

        /// <summary>
        ///班次（字典名称：manu_detail_class_type）
        /// </summary>
        public DetailClassTypeEnum DetailClassType { get; set; }

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

        /// <summary>
        ///描述
        /// </summary>
        public string? Remark { get; set; } = "";
    }

    /// <summary>
    /// 班制维护详情查询对象
    /// </summary>
    public class InteClassDetailQueryDto : PagerInfo { }
}
