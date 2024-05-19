using System;

namespace ET.Client
{
    [FriendOf(typeof(Item))]
    public class ItemAwakeSystem: AwakeSystem<Item,int>
    {
        protected override void Awake(Item self,int configID)
        {
            self.ConfigId = configID;
        }
    }
    [FriendOf(typeof(Item))]
    public class ItemDestorySystem: DestroySystem<Item>
    {
        protected override void Destroy(Item self)
        {
            self.Quality   = 0;
            self.ConfigId  = 0;
        }
    }
    

    [FriendOf(typeof(Item))]
    public static class ItemSystem
    {
        public static void FromMessage(this Item self,ItemInfo itemInfo)
        {
            //111 self.Id = itemInfo.ItemUid;
            self.ConfigId = itemInfo.ItemConfigId;
            self.Quality = itemInfo.ItemQuality;

            if (itemInfo.EquipInfo != null)
            {
                EquipInfoComponent equipInfoComponent = self.GetComponent<EquipInfoComponent>();

                if (equipInfoComponent == null)
                {
                    equipInfoComponent = self.AddComponent<EquipInfoComponent>();
                }
                equipInfoComponent.FromMessage(itemInfo.EquipInfo);
            }
        }
        
    }
}