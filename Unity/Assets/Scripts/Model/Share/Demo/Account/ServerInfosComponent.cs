using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class ServerInfosComponent : Entity,IAwake,IDestroy
    {
        public List<EntityRef<ServerInfo>> ServerInfoList = new List<EntityRef<ServerInfo>>();

        public int CurrentServerId = 0;
    }
}