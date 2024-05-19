using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
//鍛造
namespace ET.Server
{
    [ComponentOf]
    public class ForgeComponent : Entity,IAwake,IDestroy,IDeserialize,ITransfer,IUnitCache
    {
        
        [BsonIgnore]
        public Dictionary<long, long> ProductionTimerDict = new Dictionary<long, long>();
        

        [BsonIgnore]
        //生产队列
        public List<EntityRef<Production>> ProductionsList = new List<EntityRef<Production>>();
    }
}