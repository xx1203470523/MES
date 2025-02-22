﻿namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格代码关联不合格代码组视图
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupCodeRelationView
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 不合格代码组id
        /// </summary>
        public long UnqualifiedGroupId { get; set; }


        /// <summary>
        /// 不合格代码id
        /// </summary>
        public long UnqualifiedCodeId { get; set; }
        // <summary>
        /// 不合格代码
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
