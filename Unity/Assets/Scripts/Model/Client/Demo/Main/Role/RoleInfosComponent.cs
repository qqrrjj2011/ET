using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    [BsonDiscriminator("Client.RoleInfosComponent")]
    public class RoleInfosComponent : Entity ,IAwake,IDestroy
    {
        public List<EntityRef<RoleInfo>> RoleInfos = new List<EntityRef<RoleInfo>>();
        public long CurrentRoleId = 0;
    }
}