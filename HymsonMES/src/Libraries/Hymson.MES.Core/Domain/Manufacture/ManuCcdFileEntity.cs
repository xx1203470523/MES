/*
 *creator: Karl
 *
 *describe: CCD文件    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-05-17 11:09:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// CCD文件，数据实体对象   
    /// manu_ccd_file
    /// @author pengxin
    /// @date 2023-05-17 11:09:19
    /// </summary>
    public class ManuCcdFileEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 是否合格;空认为合格 1合格，0不合格
        /// </summary>
        public TrueOrFalseEnum Passed { get; set; }

        /// <summary>
        /// 上传完成路径
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 采集完成时间
        /// </summary>
        public string Timestamp { get; set; }


    }
}
