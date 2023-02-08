using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcMaterialView : ProcMaterialEntity
    {
        /// <summary>
        /// 描述 :所属物料组
        /// 空值 : false  
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 描述 :名称 (工艺路线)
        /// 空值 : true  
        /// </summary>
        public string ProcessRouteName { get; set; }

        /// <summary>
        /// 描述 :版本 (工艺路线) 
        /// 空值 : true  
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// 描述 :名称（工序Bom）
        /// 空值 : true  
        /// </summary>
        public string ProcedureBomName { get; set; }

        /// <summary>
        /// 描述 :版本（工序Bom）
        /// 空值 : true  
        /// </summary>
        public string ProcedureBomVersion { get; set; }
    }
}
