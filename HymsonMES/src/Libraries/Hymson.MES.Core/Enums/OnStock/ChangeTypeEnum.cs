using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.OnStock
{
    /// <summary>
    /// 变更类型(0-库存异动 1-储位转换 2-形态转换) 
    /// </summary>
    public enum ChangeTypeEnum:short
    {
        /// <summary>
        /// 库存异动
        /// </summary>
        StockChange = 0,

        /// <summary>
        /// 储位转换
        /// </summary>
        StorageConversion = 1,


        /// <summary>
        /// 形态转换
        /// </summary>
        MorphologicalTransformation = 2,
    }
}
