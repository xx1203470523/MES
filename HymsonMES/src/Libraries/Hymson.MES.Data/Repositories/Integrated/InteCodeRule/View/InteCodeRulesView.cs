/*
 *creator: Karl
 *
 *describe: 编码规则 页面视图类 
 *builder:  Karl
 *build datetime: 2023-03-24 14:28:17
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Core.Domain.Integrated;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 编码规则 页面视图
    /// </summary>
    public class InteCodeRulesPageView : InteCodeRulesEntity
    {
        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId {  get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 容器编码
        /// </summary>
        public string ContainerCode { get; set; }

        /// <summary>
        /// 容器名称
        /// </summary>
        public string ContainerName { get; set; }
    }
}
