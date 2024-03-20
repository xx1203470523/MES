﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Constants
{
    /// <summary>
    /// 顷刻能源错误代码15000-16000
    /// </summary>
    public static partial class ErrorCode
    {
        #region 基础通用校验

        /// <summary>
        /// 设备资源编码没有正确关联工序线体
        /// </summary>
        public const string MES45001 = "设备资源编码没有正确关联工序线体";

        /// <summary>
        /// 设备编码不能为空
        /// </summary>
        public const string MES45002 = "（EquipmentCode)设备编码字段不能为空";

        /// <summary>
        /// 资源编码不能为空
        /// </summary>
        public const string MES45003 = "（ResourceCode)资源编码字段不能为空";
        #endregion

        #region 操作员登录

        /// <summary>
        /// 设备没有维护对应的账号密码
        /// </summary>
        public const string MES45011 = "【{EquipmentCode}】设备没有维护对应的账号密码";

        /// <summary>
        /// 设备上传的账号密码错误
        /// </summary>
        public const string MES45012 = "【{EquipmentCode}】设备上传的账号密码错误";

        /// <summary>
        /// 用户名或密码不能为空
        /// </summary>
        public const string MES45013 = "用户名或密码不能为空";
        #endregion

        #region 开机参数

        /// <summary>
        /// 设备没有维护对应的开机参数
        /// </summary>
        public const string MES45021 = "设备没有维护对应的开机参数，请检查开机参数是否启用或是否关联到对应的设备组";

        /// <summary>
        /// 配方状态没有激活，请激活后在使用
        /// </summary>
        public const string MES45022 = "配方没有激活或不存在，请激活后在使用";

        /// <summary>
        /// 配方对应版本型号没有激活，请获取最新开机参数
        /// </summary>
        public const string MES45023 = "配方对应版本型号没有激活，请获取最新开机参数";
        #endregion

        #region 工单
        /// <summary>
        /// 设备所在线体没有激活工单
        /// </summary>
        public const string MES45030 = "设备所在线体没有激活工单";

        /// <summary>
        /// 设备所在线体激活不止一个工单
        /// </summary>
        public const string MES45031 = "设备所在线体激活不止一个工单，请确认";

        /// <summary>
        /// 设备所在线体工单不是生产中或者已下达
        /// </summary>
        public const string MES45032 = "设备所在线体工单不是生产中或者已下达";

        /// <summary>
        /// 工单所在BOM没有物料
        /// </summary>
        public const string MES45033 = "工单所在BOM没有物料";
        #endregion

        #region 上料
        /// <summary>
        /// 上料点没有维护对应的资源或者维护多个资源
        /// </summary>
        public const string MES45040 = "上料点没有维护对应的资源或者维护多个资源";
        #endregion

        #region 配方

        /// <summary>
        /// 设备没有对应型号为激活状态的配方
        /// </summary>
        public const string MES45050 = "设备没有对应型号为激活状态的配方";

        /// <summary>
        /// 配方没有维护具体的步骤
        /// </summary>
        public const string MES45051 = "配方没有维护具体的步骤";

        /// <summary>
        /// 配方对应版本型号没有激活
        /// </summary>
        public const string MES45052 = "配方对应版本型号没有激活，请获取最新配方";

        #endregion

        #region 请求产出极卷码

        /// <summary>
        /// 生成条码失败
        /// </summary>
        public const string MES45060 = "生成条码失败";

        #endregion

        #region 上料点

        /// <summary>
        /// 上料点不存在
        /// </summary>
        public const string MES45070 = "上料点不存在";

        /// <summary>
        /// 上料点不存在上料记录
        /// </summary>
        public const string MES45071 = "上料点不存在上料记录";

        /// <summary>
        /// 条码在上料表不存在
        /// </summary>
        public const string MES45072 = "条码在上料表不存在";

        /// <summary>
        /// 条码数量小于等于0
        /// </summary>
        public const string MES45073 = "条码数量小于等于0";

        /// <summary>
        /// 上料点或设备数据不存在或者缺少
        /// </summary>
        public const string MES45074 = "上料点或设备数据不存在或者缺少";

        #endregion

        #region 制胶匀浆

        /// <summary>
        /// 物料不在工单BOM里
        /// </summary>
        public const string MES45080 = "物料不在工单BOM里";

        /// <summary>
        /// 条码不存在或者数量小于0
        /// </summary>
        public const string MES45081 = "条码不存在或者数量小于0";

        #endregion

        #region 在制品

        /// <summary>
        /// 设备无产出条码
        /// </summary>
        public const string MES45090 = "设备无产出条码";

        /// <summary>
        /// 条码是NG状态
        /// </summary>
        public const string MES45091 = "条码是NG状态";

        #endregion

        #region 条码生成

        /// <summary>
        /// 没有配置对应的线体
        /// </summary>
        public const string MES45100 = "配置项中没有对应的线体";

        #endregion

        #region 载具

        /// <summary>
        /// 载具不存在
        /// </summary>
        public const string MES45110 = "载具不存在";

        /// <summary>
        /// 载具已经绑定条码
        /// </summary>
        public const string MES45111 = "载具已经绑定条码";

        /// <summary>
        /// 载具中条码系统不存在
        /// </summary>
        public const string MES45112 = "载具在条码系统不存在";

        /// <summary>
        /// 载具中条码数量和数据库中不匹配
        /// </summary>
        public const string MES45113 = "载具中条码数量和数据库中不匹配";

        /// <summary>
        /// 载具中条码和数据库中不匹配
        /// </summary>
        public const string MES45114 = "载具中条码和数据库中不匹配";

        /// <summary>
        /// 存在已经绑定的条码
        /// </summary>
        public const string MES45115 = "存在已经绑定的条码";

        /// <summary>
        /// 载具没有关联载具类型
        /// </summary>
        public const string MES45116 = "载具没有关联载具类型";

        /// <summary>
        /// 载具对应载具类型异常
        /// </summary>
        public const string MES45117 = "载具对应载具类型异常";

        /// <summary>
        /// 载具对应载具类型状态已禁用
        /// </summary>
        public const string MES45118 = "载具对应载具类型状态已禁用";

        /// <summary>
        /// 载具对应载具类型状态已禁用
        /// </summary>
        public const string MES45119 = "载具装载数量加上这次已超过最大值";

        /// <summary>
        /// 载具中条码和系统中不匹配，部分条码没有解绑
        /// </summary>
        public const string MES45120 = "载具中条码和系统中不匹配，部分条码没有解绑";

        /// <summary>
        /// 托盘NG中上传的不合格代码在系统中不存在
        /// </summary>
        public const string MES45121 = "托盘NG中上传的不合格代码在系统中都不存在";

        #endregion
    }
}
