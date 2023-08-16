using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 自定义 物料组 视图
    /// </summary>
    public class CustomProcMaterialGroupView : ProcMaterialGroupEntity
    {
        #region 物料相关属性
        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 描述 :物料版本 
        /// 空值 : true  
        /// </summary>
        public string? Version { get; set; }
        #endregion

    }
}
