using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(Scroll_Item_bagItem))]
    [FriendOf(typeof(Item))]
    [FriendOf(typeof(BagComponent))]
    public   static partial class Scroll_Item_bagItemSystem
    {
        public static void Refresh(this Scroll_Item_bagItem self,long id)
        {
            Item item = self.Root().GetComponent<BagComponent>().GetItemById(id);
            
            self.E_IconImage.overrideSprite = IconHelper.LoadIconSprite(self.Root().Scene(), "Icons", item.Config.Icon);
            self.E_QualityImage.color       = item.ItemQualityColor();
            self.E_SelectButton.AddListenerWithId(self.OnShowItemEntryPopUpHandler,id);
        }
        
        public static void OnShowItemEntryPopUpHandler(this Scroll_Item_bagItem self, long Id)
        {
            self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_ItemPopUp);
            Item item = self.Root().GetComponent<BagComponent>().GetItemById(Id);
            EventSystem.Instance.PublishAsync(self.Root(), new EventClientType.RefreshItemPopUp(){
                item = item,
                itemContainerType = ItemContainerType.Bag
            }).Coroutine();
            //self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgItemPopUp>().RefreshInfo(item,ItemContainerType.Bag);
        }
    }
}