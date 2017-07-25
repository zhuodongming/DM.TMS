using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Domain.Service.PKC
{
    /// <summary>
    /// 响应模型
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 状态值 1:成功、其它失败
        /// </summary>
        public string Status { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { set; get; }
    }

    /// <summary>
    /// 响应模型(泛型)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseModel<T> : ResponseModel
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
    }
}
