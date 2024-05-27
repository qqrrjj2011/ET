using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ComponentOf]
  //  [ChildOf(typeof(Item))]
    [BsonDiscriminator("Client.EquipmentsComponent")]
    public class EquipmentsComponent : Entity,IAwake,IDestroy
    {
        public Dictionary<int, EntityRef<Item>> EquipItems = new Dictionary<int, EntityRef<Item>>();
    }
}