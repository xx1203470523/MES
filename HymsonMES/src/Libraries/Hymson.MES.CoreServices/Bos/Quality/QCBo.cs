using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.CoreServices.Bos.Quality
{
    /// <summary>
    /// 处理意见Bo
    /// </summary>
    public class QCHandleBo
    {
        /// <summary>
        /// 不合格处理方式
        /// </summary>
        public HandMethodEnum HandMethod { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

    }
}
