using System.Collections;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 共享配置  
    /// Hotfix中也有一个相同的代码，改动需要两边一起改（服务器代码暂时读取不了，先这样用）
    /// </summary>
    public static partial class  AllShareConfig
    {
#if Publish
        public const string RouterHttpHost = "43.139.128.28";
        public const int RouterHttpPort = 8080;
        // 热更资源地址
        public const string hostServerIP = "http://43.139.128.28:8080/static/ET/";
        public const string appVersion = "v1.0";
#elif Local
       public const string RouterHttpHost = "127.0.0.1";
        public const int RouterHttpPort = 30300;
        public const string hostServerIP = "http://127.0.0.1";
        public const string appVersion = "v1.0"; 
#else
        public const string RouterHttpHost = "127.0.0.1";
        public const int RouterHttpPort = 30300;
        public const string hostServerIP = "http://127.0.0.1";
        public const string appVersion = "v1.0"; 
#endif
 
        public const int SessionTimeoutTime = 30 * 1000;
        
    }
}
