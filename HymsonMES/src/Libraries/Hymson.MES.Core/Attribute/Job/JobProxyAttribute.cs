using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Attribute.Job
{
    /// <summary>
    /// 作业特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobProxyAttribute : System.Attribute
    {
        public JobProxyAttribute() { }

        public Type TableEntity { get; set; }
    }

    /// <summary>
    /// 作业特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreAttribute : System.Attribute
    {
        public IgnoreAttribute()
        {
            this.IsIgnore = true;
        }

        public IgnoreAttribute(bool iIgnore)
        {
            this.IsIgnore = iIgnore;
        }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool IsIgnore { get; set; }
    }

    /// <summary>
    /// 作业特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : System.Attribute
    {
        public PrimaryKeyAttribute()
        {
            this.IsPrimaryKey = true;
        }

        public PrimaryKeyAttribute(bool isPrimaryKey)
        {
            this.IsPrimaryKey = isPrimaryKey;
        }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool IsPrimaryKey { get; set; }
    }
}
