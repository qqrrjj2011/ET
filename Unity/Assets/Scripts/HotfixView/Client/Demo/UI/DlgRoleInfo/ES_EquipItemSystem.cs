using UnityEngine;

namespace ET.Client
{
    public static partial class ES_EquipItemSystem
    {

        public static void RegisterEventHandler(this ES_EquipItem self, EquipPosition equipPosition)
        {
            self.E_SelecteButton.AddListenerWithId(self.OnSelectedHandler,(int)equipPosition);
        }


        public static void OnSelectedHandler(this ES_EquipItem self, int equipPosition)
        {
            Item item = self.Root().GetComponent<EquipmentsComponent>().GetItemByPosition((EquipPosition)equipPosition);
            if ( null == item )
            {
                return;
            }
            self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ItemPopUp);
           // self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgItemPopUp>().RefreshInfo(item,ItemContainerType.RoleInfo);
            EventSystem.Instance.PublishAsync(self.Root(), new EventClientType.RefreshItemPopUp(){
                item = item,
                itemContainerType = ItemContainerType.Bag
            }).Coroutine();
        }

        public static void RefreshShowItem(this ES_EquipItem self, EquipPosition equipPosition)
        {
            Item item = self.Root().GetComponent<EquipmentsComponent>().GetItemByPosition(equipPosition);
            if ( null != item )
            {
                self.E_IconImage.overrideSprite = IconHelper.LoadIconSprite(self.Root(),"Icons", item.Config.Icon);
                self.E_QualityImage.color       = item.ItemQualityColor();
            }
            else
            {
                self.E_IconImage.sprite = null;
                self.E_IconImage.overrideSprite = null;
                self.E_QualityImage.color       = Color.grey;
            }
        }
        
        public static void RefreshShowItem(this ES_EquipItem self, int itemConfigId)
        {
            self.E_QualityImage.color       = Color.grey;
            self.E_IconImage.overrideSprite = IconHelper.LoadIconSprite(self.Root(),"Icons", ItemConfigCategory.Instance.Get(itemConfigId).Icon);
        }        
        
        
    }
}