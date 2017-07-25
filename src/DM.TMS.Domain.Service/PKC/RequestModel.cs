using System;
using System.Collections.Generic;
using System.Text;

namespace DM.TMS.Domain.Service.PKC
{
    public class RequestModel
    {
        public RequestModel()
        {
            Version = "1.0";
            Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public string AppID { get; set; }//应用ID

        public string MethodName { get; set; }//接口方法名

        public string Version { get; set; }//版本号

        public string Timestamp { get; set; }//时间戳，格式yyyyMMddHHmmss

        public string Sign { get; set; }//签名

        public string PostData { get; set; }
    }
}
