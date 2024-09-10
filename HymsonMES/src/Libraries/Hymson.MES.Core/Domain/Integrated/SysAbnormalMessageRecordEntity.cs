using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（系统异常消息记录表）   
    /// sys_abnormal_message_record
    /// @author Yxx
    /// @date 2024-09-10 09:32:07
    /// </summary>
    public class SysAbnormalMessageRecordEntity : BaseEntity
    {
        /// <summary>
        /// 消息来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public string MessageStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark4 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark5 { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
