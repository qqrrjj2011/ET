using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 共享配置
    /// </summary>
    public static partial class  AllShareConfig
    {

        public const string RouterHttpHost = "127.0.0.1";
        public const int RouterHttpPort = 30300;
        public const string hostServerIP = "http://127.0.0.1";
        public const string appVersion = "v1.0";

        // public const string RouterHttpHost = "43.139.128.28";
        // public const int RouterHttpPort = 8080;
        // // 热更资源地址
        // public const string hostServerIP = "http://43.139.128.28:8080/static/ET/";
        // public const string appVersion = "v1.0";

        
        public const int SessionTimeoutTime = 30 * 1000;
    }
}
