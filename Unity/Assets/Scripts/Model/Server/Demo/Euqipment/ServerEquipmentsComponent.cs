using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf]
   // [ChildOf(typeof(ServerItem))]
    public class EquipmentsComponent : Entity,IAwake,IDestroy,ITransfer,IDeserialize,IUnitCache
    {
        [BsonIgnore]
        public Dictionary<int, EntityRef<ServerItem>> EquipItems = new Dictionary<int, EntityRef<ServerItem>>();
        
        [BsonIgnore]
        public M2C_ItemUpdateOpInfo message = M2C_ItemUpdateOpInfo.Create();
    }
}