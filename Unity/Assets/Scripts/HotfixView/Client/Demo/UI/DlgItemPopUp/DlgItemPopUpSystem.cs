using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgItemPopUp))]
	[FriendOf(typeof(AttributeEntry))]
	[FriendOf(typeof(EquipInfoComponent))]
	public static  class DlgItemPopUpSystem
	{

		public static void RegisterUIEvent(this DlgItemPopUp self)
		{
			self.RegisterCloseEvent<DlgItemPopUp>(self.View.E_CloseButton);
			self.View.E_EntrysLoopVerticalScrollRect.AddItemRefreshListener(self.OnEntryLoopHandler);    
			self.View.E_EquipButton.AddListenerAsync(self.OnEquipItemHandler);
			self.View.E_UnEquipButton.AddListenerAsync(self.OnUnloadEquipItemHandler);
			self.View.E_SellButton.AddListenerAsync(self.OnSellItemHandler);
		}

		public static void ShowWindow(this DlgItemPopUp self, Entity contextData = null)
		{
		}

		public static void HideWindow(this DlgItemPopUp self)
		{
			self.RemoveUIScrollItems(ref self.ScrollItemEntries);
		}
		
		
		public static void OnEntryLoopHandler(this DlgItemPopUp self,Transform transform, int index)
		{
			Scroll_Item_entry ent = self.ScrollItemEntries[index];
			Scroll_Item_entry scrollItemEntry = ent.BindTrans(transform);
			Item item = ItemHelper.GetItem(self.Root(),self.ItemId, self.ItemContainerType);
			AttributeEntry entry = item.GetComponent<EquipInfoComponent>().EntryList[index];
			scrollItemEntry.E_EntryNameText.text  = PlayerNumericConfigCategory.Instance.Get(entry.Key).Name + ":";
			bool isPrcent = PlayerNumericConfigCategory.Instance.Get(entry.Key).isPrecent > 0;
			scrollItemEntry.E_EntryValueText.text =   "+" + ( isPrcent? $"{(entry.Value/10000.0f).ToString("0.00")}%" : entry.Value.ToString());
		}

		public static void RefreshInfo(this DlgItemPopUp self, Item item,ItemContainerType itemContainerType)
		{
			self.ItemId            = item.Id;
			self.ItemContainerType = itemContainerType;
			
			self.View.E_IconImage.overrideSprite = IconHelper.LoadIconSprite(self.Root(),"Icons", item.Config.Icon);
			self.View.E_QualityImage.color       = item.ItemQualityColor();
			self.View.E_NameText.text  = item.Config.Name;
			self.View.E_DescText.text  = item.Config.Desc;
			self.View.E_PriceText.text = item.Config.SellBasePrice.ToString();
			

			if (item.Config.Type == (int)ItemType.Prop)
			{
				self.View.E_EquipButton.SetVisible(false);
				self.View.E_UnEquipButton.SetVisible(false);
				self.View.E_SellButton.SetVisible(false);
				self.View.E_EntrysLoopVerticalScrollRect.SetVisible(true,0);
				self.View.E_SellButton.SetVisible(itemContainerType == ItemContainerType.Bag);
			}
			else
			{
				self.View.E_ScoreText.text = item.GetComponent<EquipInfoComponent>().Score.ToString();
				int count = item.GetComponent<EquipInfoComponent>().EntryList.Count;
				self.AddUIScrollItems(ref self.ScrollItemEntries,count);
				self.View.E_EntrysLoopVerticalScrollRect.SetVisible(true,count);
			
				self.View.E_EquipButton.SetVisible(itemContainerType == ItemContainerType.Bag);
				self.View.E_UnEquipButton.SetVisible(itemContainerType == ItemContainerType.RoleInfo);
				self.View.E_SellButton.SetVisible(itemContainerType == ItemContainerType.Bag);
			}
		}


		public static async ETTask OnEquipItemHandler(this DlgItemPopUp self)
		{
			try
			{
				int errorCode = await ItemApplyHelper.EquipItem(self.Root(), self.ItemId);

				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}
				self.Root().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ItemPopUp);
				self.Root().GetComponent<UIComponent>().GetDlgLogic<DlgBag>().Refresh();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
		
		public static async ETTask OnUnloadEquipItemHandler(this DlgItemPopUp self)
		{
			try
			{
				int errorCode = await ItemApplyHelper.UnloadEquipItem(self.Root(), self.ItemId);

				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}
				self.Root().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ItemPopUp);
				self.Root().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>().RefreshEquipShowItems();
				//1111 self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>().Refresh();
				EventSystem.Instance.PublishAsync(self.Root(),new EventClientType.RefreshRoleInfo(){}).Coroutine();

			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
		
		public static async ETTask OnSellItemHandler(this DlgItemPopUp self)
		{
			try
			{
				int errorCode = await ItemApplyHelper.SellBagItem(self.Root(), self.ItemId);

				if (errorCode != ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}
				self.Root().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ItemPopUp);
				self.Root().GetComponent<UIComponent>().GetDlgLogic<DlgBag>().Refresh();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}
		

		 

	}
}
