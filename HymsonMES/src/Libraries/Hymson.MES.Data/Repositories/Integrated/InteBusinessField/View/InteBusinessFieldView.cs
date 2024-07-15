using Hymson.MES.Core.Domain.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Integrated.InteBusinessField.View
{
    /// <summary>
    ///  页面视图
    /// </summary>
    public class InteBusinessFieldView : InteBusinessFieldEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string MaskCode { get; set; }

        /// <summary>
        ///   名称
        /// </summary>
        public string MaskName { get; set; }
    }
}
