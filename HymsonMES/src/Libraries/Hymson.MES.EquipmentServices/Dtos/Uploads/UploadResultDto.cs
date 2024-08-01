namespace Hymson.MES.EquipmentServices.Uploads
{
    /// <summary>
    /// 上传文件结果 DTO
    /// </summary>
    public class UploadResultDto
    {
        /// <summary>
        /// 文件原名
        /// </summary>
        public string OriginalName { get; set; } = "";

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; } = "";

        /// <summary>
        /// 描述 : 文件存储地址 eg：/uploads/20220202
        /// 空值 : true  
        /// </summary>
        public string FileUrl { get; set; } = "";

        /// <summary>
        /// 描述 : 文件大小
        /// 空值 : true  
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 描述 : 文件扩展名
        /// 空值 : true  
        /// </summary>
        public string FileExt { get; set; } = "";

    }
}
