namespace ET.Client
{
    
    public class EquipmentsComponentDestrory : DestroySystem<EquipmentsComponent>
    {
        protected override void Destroy(EquipmentsComponent self)
        {
           self.Clear();
        }
    }

    [FriendOf(typeof(EquipmentsComponent))]
    public static class EquipmentsComponentSystem
    {
        public static void Clear(this EquipmentsComponent self)
        {
            ForeachHelper.Foreach(self.EquipItems, (index, item) =>
            {
                Item ent = item;
                ent?.Dispose();
            });
            self.EquipItems.Clear();
        }
        
        
        public static Item GetItemById(this EquipmentsComponent self, long itemId)
        {
            if (self.Children.TryGetValue(itemId,out Entity entity))
            {
                return entity as Item;
            }

            return null;
        }
        
        
        public static Item GetItemByPosition(this EquipmentsComponent self, EquipPosition equipPosition)
        {
            if (self.EquipItems.TryGetValue((int)equipPosition,out EntityRef<Item> item))
            {
                return item;
            }

            return null;
        }

        public static void AddEquipItem(this EquipmentsComponent self, Item item)
        {
            if (self.EquipItems.TryGetValue(item.Config.EquipPosition,out EntityRef<Item> equipItem))
            {
                Log.Error($"Already EquipItem in Postion{(EquipPosition) item.Config.EquipPosition}");
                return;
            }

            self.AddChild(item);
            self.EquipItems.Add(item.Config.EquipPosition,item);
        }
        
        public static bool IsEquipItemByPosition(this EquipmentsComponent self, EquipPosition equipPosition)
        {
            return self.EquipItems.ContainsKey((int)equipPosition);
        }
        
        public static bool UnloadEquipItem(this EquipmentsComponent self, Item item)
        {
            self.EquipItems.Remove(item.Config.EquipPosition);
            item?.Dispose();
            return true;
        }
    }
}