namespace Hymson.MES.Core.Constants
{
    public class ErrorCode
    {
        #region  用户端错误 
        public const string MES10100 = "MES10100";
        public const string MES10101 = "MES10101";
        public const string MES10001 = "MES10001";
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
        public const string MES10216 = "此物料组编码{GroupCode}在系统已经存在！";
        public const string MES10217 = "存在已分配给其他物料组的物料！";
        public const string MES10218 = "新增物料组失败！";
        public const string MES10219 = "此物料组不存在！";
        public const string MES10220 = "修改物料组失败！";
        public const string MES10221 = "已被分配物料，不允许删除！";
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
