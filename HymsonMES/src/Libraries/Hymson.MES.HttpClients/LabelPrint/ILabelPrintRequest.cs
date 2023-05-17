﻿
using Hymson.MES.HttpClients.Requests.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients
{
    /// <summary>
    /// 标签打印请求
    /// </summary>
    public interface ILabelPrintRequest
    {
        /// <summary>
        /// 打印预览图片
        /// </summary>
        /// <returns></returns>
        public Task<(string base64Str, bool result)> PreviewFromImageBase64Async(PrintRequest  printRequest);
        /// <summary>
        ///标签打印
        /// </summary>
        /// <param name="ShowDialog"></param>
        /// <returns></returns>
        public Task<(string msg, bool result)> PrintAsync(PrintRequest printRequest, bool ShowDialog = false);
    }
}