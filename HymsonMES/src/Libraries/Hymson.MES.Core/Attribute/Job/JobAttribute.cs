using Hymson.MES.Core.Enums.Job;

namespace Hymson.MES.Core.Attribute.Job
{
    /// <summary>
    /// 作业特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public JobAttribute(string name, JobTypeEnum type)
        {
            this.Type = type;
            this.Name = name;
        }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作业类型
        /// </summary>
        public JobTypeEnum Type { get; set; }
    }
}
