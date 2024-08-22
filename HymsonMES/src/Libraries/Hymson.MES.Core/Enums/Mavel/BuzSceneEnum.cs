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
        [Description("主数据（产品）推送")]
        Master_Product = 101,
        /// <summary>
        /// 主数据（产品）
        /// </summary>
        [Description("主数据（产品）汇总")]
        Master_Product_Summary = 1010,
        /// <summary>
        /// 主数据（工站） 
        /// </summary> 
        [Description("主数据（工站）推送")]
        Master_Station = 102,
        /// <summary>
        /// 主数据（工站） 
        /// </summary> 
        [Description("主数据（工站）汇总")]
        Master_Station_Summary = 1020,
        /// <summary>
        /// 主数据（控制项） 
        /// </summary> 
        [Description("主数据（控制项）推送")]
        Master_Field = 103,
        /// <summary>
        /// 主数据（控制项） 
        /// </summary> 
        [Description("主数据（控制项）汇总")]
        Master_Field_Summary = 1030,
        /// <summary>
        /// 主数据（一次合格率目标） 
        /// </summary> 
        [Description("主数据（一次合格率目标）推送")]
        Master_PassrateTarget = 104,
        /// <summary>
        /// 主数据（一次合格率目标） 
        /// </summary> 
        [Description("主数据（一次合格率目标）汇总")]
        Master_PassrateTarget_Summary = 1040,
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
        [Description("业务数据（控制项）推送")]
        Buz_Collection = 201,
        /// <summary>
        /// 业务数据（控制项）
        /// </summary> 
        [Description("业务数据（控制项）汇总")]
        Buz_Collection_Summary = 2010,
        /// <summary>
        /// 业务数据（生产业务）
        /// </summary> 
        [Description("业务数据（生产业务）推送")]
        Buz_Production = 202,
        /// <summary>
        /// 业务数据（生产业务）
        /// </summary> 
        [Description("业务数据（生产业务）汇总")]
        Buz_Production_Summary = 2020,
        /// <summary>
        /// 业务数据（材料清单）
        /// </summary> 
        [Description("业务数据（材料清单）推送")]
        Buz_Material = 203,
        /// <summary>
        /// 业务数据（材料清单）
        /// </summary> 
        [Description("业务数据（材料清单）汇总")]
        Buz_Material_Summary = 2030,
        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary> 
        [Description("业务数据（产品一次合格率）推送")]
        Buz_PassrateProduct = 204,
        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary> 
        [Description("业务数据（产品一次合格率）汇总")]
        Buz_PassrateProduct_Summary = 2040,
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
        [Description("业务数据（缺陷业务）推送")]
        Buz_Issue = 207,
        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary> 
        [Description("业务数据（缺陷业务）汇总")]
        Buz_Issue_Summary = 2070,
        /// <summary>
        /// 业务数据（工单业务）
        /// </summary> 
        [Description("业务数据（工单业务）推送")]
        Buz_WorkOrder = 208,
        /// <summary>
        /// 业务数据（工单业务）
        /// </summary> 
        [Description("业务数据（工单业务）汇总")]
        Buz_WorkOrder_Summary = 2080,
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
        /// ERP（合作伙伴精益与生产能力）
        /// </summary> 
        [Description("ERP（合作伙伴精益与生产能力）推送")]
        ERP_ProductionCapacity = 401,
        /// <summary>
        /// ERP（合作伙伴精益与生产能力）
        /// </summary> 
        [Description("ERP（合作伙伴精益与生产能力）汇总")]
        ERP_ProductionCapacity_Summary = 4010,
        /// <summary>
        /// ERP（关键下级件信息）
        /// </summary> 
        [Description("ERP（关键下级件信息）推送")]
        ERP_KeySubordinate = 402,
        /// <summary>
        /// ERP（关键下级件信息）
        /// </summary> 
        [Description("ERP（关键下级件信息）汇总")]
        ERP_KeySubordinate_Summary = 4020,
        /// <summary>
        /// ERP（实际交付情况）
        /// </summary> 
        [Description("ERP（实际交付情况）推送")]
        ERP_ActualDelivery = 403,
        /// <summary>
        /// ERP（实际交付情况）
        /// </summary> 
        [Description("ERP（实际交付情况）汇总")]
        ERP_ActualDelivery_Summary = 4030,

        /// <summary>
        /// Mock（Hello）
        /// </summary>
        [Description("Mock（Hello）")]
        Mock_Hello = 900

    }
}
