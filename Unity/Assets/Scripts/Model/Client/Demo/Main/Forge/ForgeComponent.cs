using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
//鍛造
namespace ET.Client
{
    [ComponentOf]
    [FriendOf(typeof(Production))]
    [BsonDiscriminator("Client.ForgeComponent")]
    public class ForgeComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<long, long> ProductionTimerDict = new Dictionary<long, long>();
        //生产队列
        public List<EntityRef<Production>> ProductionsList = new List<EntityRef<Production>>();
    }
}