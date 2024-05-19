
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(EquipmentsComponent))]
    [FriendOf(typeof(ServerBagComponent))]
    [FriendOf(typeof(ServerItem))]
    public static class ItemUpdateNoticeHelper
    {
        public static void SyncAddItem(Unit unit, ServerItem item,  M2C_ItemUpdateOpInfo message)
        {
            message.ItemInfo = item.ToMessage();
            message.Op =(int)ItemOp.Add;
            MapMessageHelper.SendToClient(unit, message);
        }
        
        public static void SyncRemoveItem(Unit unit, ServerItem item,M2C_ItemUpdateOpInfo message)
        {
            message.ItemInfo = item.ToMessage(false);
            message.Op = (int)ItemOp.Remove;
            MapMessageHelper.SendToClient(unit, message);
        }


        public static void SyncAllBagItems(Unit unit)
        {
            M2C_AllItemsList m2CAllItemsList = M2C_AllItemsList.Create();
            m2CAllItemsList.ContainerType = (int)ItemContainerType.Bag;
            m2CAllItemsList.ItemInfoList = new List<ItemInfo>();
            ServerBagComponent bagComponent = unit.GetComponent<ServerBagComponent>();
            foreach (ServerItem item in bagComponent.ItemsDict.Values)
            {
                m2CAllItemsList.ItemInfoList.Add(item.ToMessage());
            }
            
            MapMessageHelper.SendToClient(unit, m2CAllItemsList);
        }
        
        public static void SyncAllEquipItems(Unit unit)
        {
            M2C_AllItemsList m2CAllItemsList = M2C_AllItemsList.Create();
            m2CAllItemsList.ContainerType = (int)ItemContainerType.RoleInfo;
            m2CAllItemsList.ItemInfoList = new List<ItemInfo>();
            EquipmentsComponent equipmentsComponent = unit.GetComponent<EquipmentsComponent>();
            foreach (ServerItem item in equipmentsComponent.EquipItems.Values)
            {
                m2CAllItemsList.ItemInfoList.Add(item.ToMessage());
            }
            MapMessageHelper.SendToClient(unit, m2CAllItemsList);
        }
        
    }
}