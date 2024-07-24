using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Warehouse
{
    /// <summary>
    ///  条码关系
    /// </summary>
    public class WhMaterialStandingBookRelationDto
    {
        /// <summary>
        /// 父级条码
        /// </summary>
        public string ParentBarcode { get; set; }

        /// <summary>
        /// 子级条码
        /// </summary>
        public string ChildrenParentBarcode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 上料
    /// </summary>
    public class WhMaterialStandingBookFeedingDto
    {
        /// <summary>
        /// 上料资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 上料资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 上料点名称
        /// </summary>
        public string? LoadingPointCode { get; set; }

        /// <summary>
        /// 上料点名称
        /// </summary>
        public string? LoadingPointName { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal Qty { get; set; }
    }

}