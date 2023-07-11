using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Common.MasterData
{
    /// <summary>
    /// 生产设置
    /// </summary>
    public  class ProductSetBo
    {
        /// <summary>
        /// 工厂id
        /// </summary>
        public long SiteId{ set; get; }
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { set; get; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { set; get; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { set; get; }
    }
}
