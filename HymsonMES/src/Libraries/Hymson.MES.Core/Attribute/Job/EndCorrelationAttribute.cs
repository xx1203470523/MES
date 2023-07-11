using Hymson.MES.Core.Enums.Job;

namespace Hymson.MES.Core.Attribute.Job
{
    /// <summary>
    /// 关联点
    /// </summary>
    public class EndCorrelationAttribute : System.Attribute
    {
        public EndCorrelationAttribute(ConnectionTypeEnum connectionType)
        {
            this.ConnectionType = connectionType;
        }

        /// <summary>
        /// 关联类型
        /// </summary>
        public ConnectionTypeEnum ConnectionType { get; set; } = ConnectionTypeEnum.procedureAndResource;
    }
}
