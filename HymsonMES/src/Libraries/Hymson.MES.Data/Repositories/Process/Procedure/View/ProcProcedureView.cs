using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcProcedureView: ProcProcedureEntity
    {
        /// <summary>
        /// 描述 :资源类型 
        /// 空值 : false  
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 描述 :资源类型名称 
        /// 空值 : false  
        /// </summary>
        public string ResTypeName { get; set; }
    }
}
