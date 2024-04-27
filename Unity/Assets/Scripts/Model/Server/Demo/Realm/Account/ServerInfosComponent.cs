using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class ServerInfosComponent : Entity,IAwake,IDestroy
    {
        public List<EntityRef<ServerInfo>> ServerInfoList = new List<EntityRef<ServerInfo>>();

        
    }
}