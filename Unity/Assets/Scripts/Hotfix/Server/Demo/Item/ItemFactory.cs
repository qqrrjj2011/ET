using System;

namespace ET.Server
{
    [FriendOf(typeof(ServerItem))]
    public static class ItemFactory
    {
        [EnableAccessEntiyChild]
        public static ServerItem Create(Entity parent, int configId)
        {
            if ( !ItemConfigCategory.Instance.Contain(configId))
            {
                Log.Error($"当前所创建的物品id 不存在: {configId}");
                return null;
            }
            ServerItem item = parent.AddChild<ServerItem, int>(configId);
            item.RandomQuality();
            AddComponentByItemType(item);
            return item;
        }

        public static void AddComponentByItemType(ServerItem item)
        {
            switch ((ItemType)item.Config.Type)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Ring:
                {
                    item.AddComponent<EquipInfoComponent>();
                }
                    break;
                case ItemType.Prop:
                {
                    
                }
                    break;
            }
        }

      
        
    }
}