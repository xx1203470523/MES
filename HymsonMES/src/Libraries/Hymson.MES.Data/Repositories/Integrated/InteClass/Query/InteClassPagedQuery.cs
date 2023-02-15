using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.InteClass.Query
{
    /// <summary>
    /// 班制维护 分页参数
    /// </summary>
    public class InteClassPagedQuery : PagerInfo
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
