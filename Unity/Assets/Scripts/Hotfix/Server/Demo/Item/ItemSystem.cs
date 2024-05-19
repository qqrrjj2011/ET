namespace ET.Server
{
    [FriendOf(typeof(ServerItem))]
    public class ItemAwakeSystem: AwakeSystem<ServerItem,int>
    {
        protected override void Awake(ServerItem self,int configID)
        {
            self.ConfigId = configID;
        }
    }
    
    [FriendOf(typeof(ServerItem))]
    public class ItemDestorySystem: DestroySystem<ServerItem>
    {
        protected override void Destroy(ServerItem self)
        {
            self.Quality = 0;
            self.ConfigId = 0;
        }
    }
    
    
    [FriendOf(typeof(ServerItem))]
    public static class ItemSystem
    {
        public static ItemInfo ToMessage(this ServerItem self,bool isAllInfo = true)
        {
            ItemInfo itemInfo = ItemInfo.Create();
            itemInfo.ItemUid = self.Id;
            itemInfo.ItemConfigId = self.ConfigId;
            itemInfo.ItemQuality  = self.Quality;

            if (!isAllInfo)
            {
                return itemInfo;
            }
            
            EquipInfoComponent equipInfoComponent = self.GetComponent<EquipInfoComponent>();
            if (equipInfoComponent != null)
            {
                itemInfo.EquipInfo = equipInfoComponent.ToMessage();
            }

            return itemInfo;
        }
        
    }
}