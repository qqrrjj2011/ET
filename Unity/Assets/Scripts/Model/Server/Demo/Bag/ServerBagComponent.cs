using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf]
   // [ChildOf(typeof(ServerItem))]
    public class ServerBagComponent : Entity,IAwake,IDestroy,IDeserialize,ITransfer,IUnitCache
    {
        [BsonIgnore]
        public Dictionary<long, EntityRef<ServerItem>> ItemsDict = new Dictionary<long, EntityRef<ServerItem>>(); 

        [BsonIgnore]
        public MultiMap<int, EntityRef<ServerItem>> ItemsMap = new MultiMap<int, EntityRef<ServerItem>>();

        [BsonIgnore]
        public M2C_ItemUpdateOpInfo message = M2C_ItemUpdateOpInfo.Create();
        
  
    }
}