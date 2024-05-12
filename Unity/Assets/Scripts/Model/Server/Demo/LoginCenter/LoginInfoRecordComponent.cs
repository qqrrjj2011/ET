using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class LoginInfoRecordComponent:Entity,IAwake,IDestroy
    {
        /// <summary>
        /// account-serverID(zone)
        /// </summary>
        public Dictionary<string, int> AccountLoginInfoDict = new Dictionary<string, int>();
    }
}

