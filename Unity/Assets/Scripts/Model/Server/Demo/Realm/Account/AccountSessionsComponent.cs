using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent: Entity,IAwake,IDestroy
    {
        /// <summary>
        /// accountId-sessionInstanceId
        /// </summary>
        public Dictionary<long, EntityRef<Session>> AccountSessionDictionary = new Dictionary<long, EntityRef<Session>>();
    }
}

