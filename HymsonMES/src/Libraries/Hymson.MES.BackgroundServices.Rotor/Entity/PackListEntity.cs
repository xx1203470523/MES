using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Entity
{
    /// <summary>  
    /// 打包清单表  
    /// </summary>  
    public class PackListEntity
    {
        /// <summary>  
        /// 主键ID  
        /// </summary>  
        public string ID { get; set; }

        /// <summary>  
        /// 箱码  
        /// </summary>  
        public string BoxCode { get; set; }

        /// <summary>  
        /// 产品代码  
        /// </summary>  
        public string ProductCode { get; set; }

        /// <summary>  
        /// 产品编号  
        /// </summary>  
        public string ProductNo { get; set; }

        /// <summary>  
        /// 是否已删除  
        /// </summary>  
        public bool IsDeleted { get; set; }

        /// <summary>  
        /// 创建时间  
        /// </summary>  
        public DateTime CreateTime { get; set; }

        /// <summary>  
        /// 更新时间  
        /// </summary>  
        public DateTime UpdateTime { get; set; }

        /// <summary>  
        /// 创建人ID  
        /// </summary>  
        public string CreateID { get; set; }

        /// <summary>  
        /// 创建人姓名  
        /// </summary>  
        public string CreateName { get; set; }

        /// <summary>  
        /// 更新人ID  
        /// </summary>  
        public string UpdateID { get; set; }

        /// <summary>  
        /// 更新人姓名  
        /// </summary>  
        public string UpdateName { get; set; }
    }
}
