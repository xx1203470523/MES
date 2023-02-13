namespace Hymson.MES.Core.Constants
{
    public class ErrorCode
    {
        #region  用户端错误 
        public const string MES10100 = "请求实体不能为空";
        public const string MES10101 = "站点码获取失败，请重新登录！";
        public const string MES10102 = "删除失败Id 不能为空!";

        #region 物料 10200
        public const string MES10200 = "物料维护错误";
        public const string MES10201 = "此物料编码{materialCode}版本{version}在系统已经存在！";
        public const string MES10202 = "添加物料表失败！";
        public const string MES10203 = "站点码获取失败，请重新登录！";
        public const string MES10204 = "此物料不存在！";
        public const string MES10205 = "来源不允许修改！";
        public const string MES10206 = "替代品已包含当前物料！！";
        public const string MES10207 = "替代品操作类型OperationType:{operationType}异常，只能传入1，2，3！";
        public const string MES10208 = "修改物料表失败！";
        public const string MES10209 = "插入物料替代组件表失败！";
        public const string MES10210 = "修改物料替代组件表失败！";
        public const string MES10211 = "删除物料替代组件表失败！";
        public const string MES10212 = "有生产中工单引用当前物料，不能删除！";
        public const string MES10213 = "参数不能为空";
        public const string MES10214 = "物料编码不能为空";
        public const string MES10215 = "物料名称不能为空";
        #endregion

        #region 资源 10300
        public const string MES10301 = "资源编码不能为空";
        public const string MES10302 = "资源编码超长";
        public const string MES10303 = "资源名称不能为空";
        public const string MES10304 = "资源名称超长";
        public const string MES10305 = "资源状态不能为空";
        public const string MES10306 = "资源配置打印机中，重复配置设备!";
        public const string MES10307 = "一个资源只能对用对应一个主设备";
        public const string MES10308 = $"此资源在系统中已经存在!";
        public const string MES10309= "此资源类型不存在!";
        public const string MES10310 = "资源类型在系统中不存在,请重新输入!";
        public const string MES10311 = $"此资源类型在系统中已经存在!";
        public const string MES10312 = $"资源类型有被分配的资源，不允许删除!";
        public const string MES10313 = $"资源配置打印机中，重复配置打印机!";
        public const string MES10314 = $"资源配置设备中，重复配置设备!";
        public const string MES10315 = $"资源打印配置操作类型OperationType异常，只能传入1，2，3！";
        public const string MES10316 = $"设备绑定设置数据操作类型OperationType异常，只能传入1，2，3！";
        public const string MES10317 = $"资源设置数据操作类型OperationType异常，只能传入1，2，3！";
        public const string MES10318 = $"作业设置数据操作类型OperationType异常，只能传入1，2，3！";
        public const string MES10319 = $"不能删除启用状态的资源!";
        #endregion

        #region 工序 10400
        public const string MES10401 = "工序编码不能为空";
        public const string MES10402 = "工序编码超长";
        public const string MES10403 = "工序名称不能为空";
        public const string MES10404 = "工序名称超长";
        public  string MES10405 = $"编码:{0}已存在！";
        #endregion
        #endregion

        #region 系统执行出错 业务逻辑出错
        public const string MES20001 = "MES20001";
        public const string MES20100 = "MES20100";
        public const string MES20101 = "MES20101";
        #endregion


        #region 调用第三方服务出错
        public const string MES30001 = "MES30001";
        public const string MES30100 = "MES30100";
        public const string MES30101 = "MES30101";
        #endregion
    }
}
