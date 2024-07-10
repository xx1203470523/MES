using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Mavel
{
    /// <summary>
    /// 业务场景
    /// </summary>
    public enum BuzSceneEnum
    {
        /// <summary>
        /// 总开关
        /// </summary>
        [Description("总开关")]
        All = 0,

        /// <summary>
        /// 主数据（产品）
        /// </summary>
        [Description("主数据（产品）")]
        Master_Product = 101,
        /// <summary>
        /// 主数据（工站） 
        /// </summary> 
        [Description("主数据（工站）")]
        Master_Station = 102,
        /// <summary>
        /// 主数据（控制项） 
        /// </summary> 
        [Description("主数据（控制项）")]
        Master_Field = 103,
        /// <summary>
        /// 主数据（一次合格率目标） 
        /// </summary> 
        [Description("主数据（一次合格率目标）")]
        Master_PassrateTarget = 104,
        /// <summary>
        /// 主数据（环境监测） 
        /// </summary> 
        [Description("主数据（环境监测）")]
        Master_EnvField = 105,
        /// <summary>
        /// 主数据（人员资质） 
        /// </summary> 
        [Description("主数据（人员资质）")]
        Master_PersonCert = 106,
        /// <summary>
        /// 主数据（排班） 
        /// </summary> 
        [Description("主数据（排班）")]
        Master_TeamScheduling = 107,


        /// <summary>
        /// 业务数据（控制项）
        /// </summary> 
        [Description("业务数据（控制项）")]
        Buz_Collection = 201,
        /// <summary>
        /// 业务数据（生产业务）
        /// </summary> 
        [Description("业务数据（生产业务）")]
        Buz_Production = 202,
        /// <summary>
        /// 业务数据（材料清单）
        /// </summary> 
        [Description("业务数据（材料清单）")]
        Buz_Material = 203,
        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary> 
        [Description("业务数据（产品一次合格率）")]
        Buz_PassrateProduct = 204,
        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary> 
        [Description("业务数据（工位一次合格率）")]
        Buz_PassrateStation = 205,
        /// <summary>
        /// 业务数据（环境业务）
        /// </summary> 
        [Description("业务数据（环境业务）")]
        Buz_DataEnv = 206,
        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary> 
        [Description("业务数据（缺陷业务）")]
        Buz_Issue = 207,
        /// <summary>
        /// 业务数据（工单业务）
        /// </summary> 
        [Description("业务数据（工单业务）")]
        Buz_WorkOrder = 208,
        /// <summary>
        /// 业务数据（通用业务）
        /// </summary> 
        [Description("业务数据（通用业务）")]
        Buz_Common = 209,
        /// <summary>
        /// 业务数据（附件）
        /// </summary> 
        [Description("业务数据（附件）")]
        Buz_Attachment = 210,


        /// <summary>
        /// 文件（获取预授权上传URL）
        /// </summary> 
        [Description("文件（获取预授权上传URL）")]
        File_DisposableUpload = 301,
        /// <summary>
        /// 文件（获取附件浏览URL）
        /// </summary> 
        [Description("文件（获取附件浏览URL）")]
        File_AuthorizeUrl = 302,


        /// <summary>
        /// Mock（Hello）
        /// </summary>
        [Description("Mock（Hello）")]
        Mock_Hello = 900

    }
}
