using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public class PackagingDto
    {

    }

    /// <summary>
    /// 容器
    /// </summary>
    public class ManuContainerBarcodeViewDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 状态;1：打开 2：关闭
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public LevelEnum? Level { get; set; }

        /// <summary>
        /// 当前装载数量
        /// </summary>
        public int CurrentQuantity{get;set;}
    }
}
