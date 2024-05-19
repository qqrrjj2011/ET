namespace ET.Server
{
    [FriendOf(typeof(EquipmentsComponent))]
    public class EquipmentsComponentDestroy : DestroySystem<EquipmentsComponent>
    {
        protected override void Destroy(EquipmentsComponent self)
        {
            foreach (ServerItem item in self.EquipItems.Values)
            {
                item?.Dispose();
            }
            self.EquipItems.Clear();
            self.message = null;
        }
    }

    [FriendOf(typeof(EquipmentsComponent))]
    public class EquipmentsComponentDeserializeSystem : DeserializeSystem<EquipmentsComponent>
    {
        protected override void Deserialize(EquipmentsComponent self)
        {
            foreach (var entity in self.Children.Values)
            {
                ServerItem item = entity as ServerItem;
                self.EquipItems.Add(item.Config.EquipPosition,item);
            }
        }
    }

    
    [FriendOf(typeof(EquipmentsComponent))]
    public static class EquipmentsComponentSystem
    {
        /// <summary>
        /// 对应位置处是否有装配Item
        /// </summary>
        /// <param name="self"></param>
        /// <param name="equipPosition"></param>
        /// <returns></returns>
        public static bool IsEquipItemByPosition(this EquipmentsComponent self, EquipPosition equipPosition)
        {
            self.EquipItems.TryGetValue((int)equipPosition, out EntityRef<ServerItem> item);

            ServerItem ent = item;
            return ent != null && !ent.IsDisposed;
        }
        
        /// <summary>
        /// 装配Item
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool EquipItem(this EquipmentsComponent self, ServerItem item)
        {
            if (!self.EquipItems.ContainsKey(item.Config.EquipPosition))
            {
                self.AddChild(item);
                self.EquipItems.Add(item.Config.EquipPosition,item);
                EventSystem.Instance.Publish(self.Root(),new EventType.ChangeEquipItem(){Unit = self.GetParent<Unit>(),Item = item,EquipOp = EquipOp.Load});
                self.message.ContainerType = (int)ItemContainerType.RoleInfo;
                ItemUpdateNoticeHelper.SyncAddItem(self.GetParent<Unit>(),item,self.message);
                return true;
            }
            return false;
        }
        
        
        /// <summary>
        /// 卸下对应位置的Item
        /// </summary>
        /// <param name="self"></param>
        /// <param name="equipPosition"></param>
        /// <returns></returns>
        public static ServerItem UnloadEquipItemByPosition(this EquipmentsComponent self, EquipPosition equipPosition)
        {
            if (self.EquipItems.TryGetValue((int)equipPosition,out EntityRef<ServerItem> item))
            {
                self.EquipItems.Remove((int)equipPosition);
                EventSystem.Instance.Publish(self.Root(),new EventType.ChangeEquipItem(){Unit = self.GetParent<Unit>(),Item = item,EquipOp = EquipOp.Unload});
                self.message.ContainerType = (int)ItemContainerType.RoleInfo;
                ItemUpdateNoticeHelper.SyncRemoveItem(self.GetParent<Unit>(),item,self.message);
            }
            return item;
        }
        
        public static ServerItem GetEquipItemByPosition(this EquipmentsComponent self, EquipPosition equipPosition)
        {
            if (!self.EquipItems.TryGetValue((int)equipPosition,out EntityRef<ServerItem> item))
            {
                return null;
            }
            return item;
        }
        
        
    }
}