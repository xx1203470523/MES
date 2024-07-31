namespace Hymson.MES.Data
{
    /// <summary>
    /// 应用过缓存的表名 常量
    /// </summary>
    public static class CachedTables
    {
        /// <summary>
        /// 设备表
        /// </summary>
        public const string EQU_EQUIPMENT= "equ_equipment";

        /// <summary>
        /// 事件维护表
        /// </summary>
        public const string INTE_EVENT_TYPE = "inte_event_type";

        /// <summary>
        /// 作业表
        /// </summary>
        public const string INTE_JOB = "inte_job";

        /// <summary>
        /// 时间通配（转换）表
        /// </summary>
        public const string INTE_TIME_WILDCARD = "inte_time_wildcard";

        /// <summary>
        /// 工作中心表
        /// </summary>
        public const string INTE_WORK_CENTER = "inte_work_center";

        /// <summary>
        /// 容器装载表
        /// </summary>
        public const string MANU_CONTAINER_PACK = "manu_container_pack";

        /// <summary>
        /// 操作面板按钮表
        /// </summary>
        public const string MANU_FACE_PLATE_BUTTON = "manu_face_plate_button";

        /// <summary>
        /// 操作面板按钮作业表
        /// </summary>
        public const string MANU_FACE_PLATE_BUTTON_JOB_RELATION = "manu_face_plate_button_job_relation";

        /// <summary>
        /// 工单激活（物理删除）表
        /// </summary>
        public const string PLAN_WORK_ORDER_BIND = "plan_work_order_bind";

        /// <summary>
        /// BOM明细表
        /// </summary>
        public const string PROC_BOM_DETAIL= "proc_bom_detail";

        /// <summary>
        /// 物料维护表
        /// </summary>
        public const string PROC_MATERIAL= "proc_material";

        /// <summary>
        /// 物料替代组件表
        /// </summary>
        public const string PROC_REPLACE_MATERIAL = "proc_replace_material";

        /// <summary>
        /// 标准参数表
        /// </summary>
        public const string PROC_PARAMETER = "proc_parameter";

        /// <summary>
        /// 工序表
        /// </summary>
        public const string PROC_PROCEDURE= "proc_procedure";

        /// <summary>
        /// 工艺路线工序节点关系明细表(前节点多条就存多条)
        /// </summary>
        public const string PROC_PROCESS_ROUTE_DETAIL_LINK= "proc_process_route_detail_link";

        /// <summary>
        /// 工艺路线工序节点明细表
        /// </summary>
        public const string PROC_PROCESS_ROUTE_DETAIL_NODE = "proc_process_route_detail_node";

        /// <summary>
        /// 资源维护表
        /// </summary>
        public const string PROC_RESOURCE= "proc_resource";

        /// <summary>
        /// 工单表
        /// </summary>
        public const string PLAN_WORK_ORDER = "plan_work_order";
    }
}
