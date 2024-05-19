using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf]
    public class ServerItem : Entity,IAwake<int>,IDestroy,ISerializeToEntity
    {
        //物品配置ID
        public int ConfigId = 0;

        /// <summary>
        /// 物品品质
        /// </summary>
        public int Quality  = 0;
        
        //物品配置数据
        [BsonIgnore]
        public ItemConfig Config => ItemConfigCategory.Instance.Get(ConfigId);
        
    }
}