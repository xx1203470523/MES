namespace Hymson.MES.Core.Constants
{
    public class ErrorCode
    {
        #region  用户端错误 
        public const string WMS10100 = "WMS10100";
        public const string WMS10101 = "WMS10101";
        public const string WMS10001 = "WMS10001";
        public const string WMS10200 = "物料维护错误";
        public const string WMS10201 = "此物料编码{materialCode}版本{version}在系统已经存在！";
        public const string WMS10202 = "添加物料表失败！";
        public const string WMS10203 = "站点码获取失败，请重新登录！";
        public const string WMS10204 = "此物料不存在！";
        public const string WMS10205 = "来源不允许修改！";
        public const string WMS10206 = "替代品已包含当前物料！！";
        public const string WMS10207 = "替代品操作类型OperationType:{operationType}异常，只能传入1，2，3！";
        public const string WME10208 = "修改物料表失败！";
        public const string WME10209 = "插入物料替代组件表失败！";
        public const string WME10210 = "修改物料替代组件表失败！";
        public const string WME10211 = "删除物料替代组件表失败！";
        public const string WME10212 = "有生产中工单引用当前物料，不能删除！";
        public const string WME10213 = "参数不能为空";
        public const string WME10214 = "物料编码不能为空";
        public const string WME10215 = "物料名称不能为空";
        

        #endregion

        #region 系统执行出错 业务逻辑出错
        public const string WMS20001 = "WMS20001";
        public const string WMS20100 = "WMS20100";
        public const string WMS20101 = "WMS20101";
        #endregion


        #region 调用第三方服务出错
        public const string WMS30001 = "WMS30001";
        public const string WMS30100 = "WMS30100";
        public const string WMS30101 = "WMS30101";
        #endregion
    }
}
