using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据表数据实体对象
    /// @author admin
    /// @date 2024-05-08
    /// </summary>
    public class ProcPrintSetupEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 描述 :物料编码
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :配置类型
        /// 空值 : false  
        /// </summary>
        public PrintSetupEnum Type { get; set; }

        /// <summary>
        /// 描述:资源ID
        /// 空值 : true  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述:程序名
        /// 空值 : true  
        /// </summary>
        public string? Program { get; set; }

        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public CodeRuleCodeTypeEnum BusinessType { get; set; }

        /// <summary>
        /// 描述 :打印机ID
        /// 空值 : true
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :打印模板ID
        /// 空值 : false  
        /// </summary>
        public long LabelTemplateId { get; set; }

        /// <summary>
        /// 描述：份数
        /// 空值 : false  
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 备注
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 描述 :模板文件
        /// 空值 : true  
        /// </summary>
        /// 
        public string Content { get; set; }
        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 描述 :版本
        /// 空值 : true  
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 描述 :资源编码
        /// 空值 : true  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :模板名称
        /// 空值 : true  
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 打印机
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印机
        /// </summary>
        public string Class { get; set; }



    }

}