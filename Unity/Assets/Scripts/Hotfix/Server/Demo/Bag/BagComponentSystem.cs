
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ServerBagComponent))]
    public class BagComponentDestroySystem: DestroySystem<ServerBagComponent>
    {
        protected override void Destroy(ServerBagComponent self)
        {
            foreach (ServerItem item in self.ItemsDict.Values)
            {
                item?.Dispose();
            }
            self.ItemsDict.Clear();
            self.ItemsMap.Clear();
        }
    }
    
    public class BagComponentDeserializeSystem: DeserializeSystem<ServerBagComponent>
    {
        protected override void Deserialize(ServerBagComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                self.AddContainer(entity as ServerItem);
            }
        }
    }

    [FriendOf(typeof(ServerItem))]
    [FriendOf(typeof(ServerBagComponent))]
    public static class BagComponentSystem
    {
        
        /// <summary>
        /// 是否达到最大负载
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsMaxLoad(this ServerBagComponent self)
        {
            return self.ItemsDict.Count == self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.MaxBagCapacity];
        }
        
        public static bool AddContainer(this ServerBagComponent self, ServerItem item)
        {
            if (self.ItemsDict.ContainsKey(item.Id))
            {
                return false;
            }
            
            self.ItemsDict.Add(item.Id, item);
            self.ItemsMap.Add(item.Config.Type, item);
            return true;
        }

        public static void RemoveContainer(this ServerBagComponent self, ServerItem item)
        {
            self.ItemsDict.Remove(item.Id);
            self.ItemsMap.Remove(item.Config.Type, item);
        }
        
        
        public static bool AddItemByConfigId(this ServerBagComponent self, int configId, int count = 1)
        {
            if ( !ItemConfigCategory.Instance.Contain(configId))
            {
                return false;
            }

            if ( count <= 0 )
            {
                return false;
            }

            for ( int i = 0; i < count; i++ )
            {
                ServerItem newItem = ItemFactory.Create(self, configId);
              
                if (!self.AddItem(newItem))
                {
                    Log.Error("添加物品失败！");
                    newItem?.Dispose();
                    return false;
                }
            }

            return true;
        }
        
        public static void GetItemListByConfigId(this ServerBagComponent self, int configID, List<ServerItem> list)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(configID);
            foreach (ServerItem goods in self.ItemsMap[itemConfig.Type])
            {
                if (goods.ConfigId == configID)
                {
                    list.Add(goods);
                }
            }
        }
        
        
        public static bool IsCanAddItem(this ServerBagComponent self, ServerItem item)
        {
            if (item == null || item.IsDisposed)
            {
                return false;
            }
            
            if ( !ItemConfigCategory.Instance.Contain(item.ConfigId))
            {
                return false;
            }

            if (self.IsMaxLoad())
            {
                return false;
            }

            if (self.ItemsDict.ContainsKey(item.Id))
            {
                return false;
            }

            if (item.Parent == self)
            {
                return false;
            }
            return true;
        }
        
        
        public static bool IsCanAddItemByConfigId(this ServerBagComponent self, int configID)
        {
            if (!ItemConfigCategory.Instance.Contain(configID))
            {
                return false;
            }

            if (self.IsMaxLoad())
            {
                return false;
            }
            
            return true;
        }
        
        public static bool IsCanAddItemList(this ServerBagComponent self, List<ServerItem> goodsList)
        {
            if (goodsList.Count <= 0)
            {
                return false;
            }

            if (self.ItemsDict.Count + goodsList.Count > self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.MaxBagCapacity])
            {
                return false;
            }

            foreach (var item in goodsList)
            {
                if (item == null || item.IsDisposed)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool AddItemList(this ServerBagComponent self, List<ServerItem> itemsList)
        {
            if (itemsList.Count <= 0)
            {
                return true;
            }
            
            foreach ( ServerItem newItem in itemsList )
            {
                if (!self.AddItem(newItem) )
                {
                    newItem?.Dispose();
                    return false;
                }
            }
            return true;
        }
        
        public static bool AddItem(this ServerBagComponent self, ServerItem item )
        {
            if (item == null || item.IsDisposed)
            {
                Log.Error("item is null!");
                return false;
            }
            
            if (self.IsMaxLoad())
            {
                Log.Error("bag is IsMaxLoad!");
                return false;
            }
            
            if ( !self.AddContainer(item) )
            {
                Log.Error("Add Container is Error!");
                return false;
            }
            
            if (item.Parent != self)
            {
                self.AddChild(item);
            }

            self.message.ContainerType = (int)ItemContainerType.Bag;
            ItemUpdateNoticeHelper.SyncAddItem(self.GetParent<Unit>(), item,self.message);
            return true;
        }
        
        public static void RemoveItem(this ServerBagComponent self, ServerItem item)
        {
            self.RemoveContainer(item);
            self.message.ContainerType = (int)ItemContainerType.Bag;

            ItemUpdateNoticeHelper.SyncRemoveItem(self.GetParent<Unit>(), item,self.message);
            item.Dispose();
        }

        public static ServerItem RemoveItemNoDispose(this ServerBagComponent self, ServerItem item)
        {
            self.RemoveContainer(item);
            self.message.ContainerType = (int)ItemContainerType.Bag;

            ItemUpdateNoticeHelper.SyncRemoveItem(self.GetParent<Unit>(), item,self.message);
            return item;
        }


        public static bool IsItemExist(this ServerBagComponent self, long itemId)
        {
            self.ItemsDict.TryGetValue(itemId, out EntityRef<ServerItem> item);
            ServerItem ent = item;
            return ent != null && !ent.IsDisposed;
        }
        
        
        public static ServerItem GetItemById(this ServerBagComponent self, long itemId)
        {
            self.ItemsDict.TryGetValue(itemId, out EntityRef<ServerItem> item);
            ServerItem ent = item;
            return ent;
        }
        
    }
}