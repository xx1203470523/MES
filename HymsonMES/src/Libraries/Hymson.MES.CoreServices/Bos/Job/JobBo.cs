using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class JobBo
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { get; set; } = "";
    }

    /// <summary>
    /// 请求Bo
    /// </summary>
    public class JobRequestBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; } 
        /// <summary>
        ///  容器ID
        /// </summary>
        public long ContainerId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long[]? UnqualifiedIds { get; set; }

        /// <summary>
        /// 不合格工艺路线id
        /// </summary>
        public long? BadProcessRouteId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 响应Bo
    /// </summary>
    public class JobResponseBo
    {
        /// <summary>
        /// 影响行数
        /// </summary>
        public int Rows { get; set; } = 0;

        /// <summary>
        /// 内容
        /// </summary>
        public Dictionary<string, string> Content { get; set; } = new();

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

    }

}
