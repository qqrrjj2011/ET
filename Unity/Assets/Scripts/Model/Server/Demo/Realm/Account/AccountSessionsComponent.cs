using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class AccountSessionsComponent: Entity,IAwake,IDestroy
    {
        /// <summary>
        /// account-GateSessionKey(token)
        /// </summary>
        public Dictionary<string, EntityRef<Session>> AccountSessionDictionary = new Dictionary<string, EntityRef<Session>>();
    }
}

