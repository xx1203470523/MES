namespace Hymson.MES.Core.Attribute.Job
{
    /// <summary>
    /// 作业特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JobProxyAttribute : System.Attribute
    {
        public JobProxyAttribute(Type tableEntity)
        {
            this.TableEntity = tableEntity;
        }

        public Type TableEntity { get; set; }
    }

    /// <summary>
    /// 作业忽略特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
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
    /// 查询键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConditionAttribute : System.Attribute
    {
        public ConditionAttribute()
        {
            this.IsCondition = true;
        }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool IsCondition { get; set; }
    }

    /// <summary>
    /// 作业字段名字特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : System.Attribute
    {
        public FieldAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public string Name { get; set; }
    }
}
