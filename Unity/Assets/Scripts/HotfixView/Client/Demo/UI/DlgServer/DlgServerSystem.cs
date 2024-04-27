using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgServer))]
	[FriendOf(typeof(ClientServerInfosComponent))]
	[FriendOf(typeof(ServerInfo))]
	public static  class DlgServerSystem
	{

		public static void RegisterUIEvent(this DlgServer self)
		{
			self.View.EButton_EnterButton.AddListenerAsync(() => { return self.OnConfirmClickHandler();});
			self.View.ELoopScrollList_SeverListLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int index) => { self.OnScrollItemRefreshHandler(transform, index); });
		}

		public static void ShowWindow(this DlgServer self, Entity contextData = null)
		{
			int count = self.Root().GetComponent<ClientServerInfosComponent>().ServerInfoList.Count;
			self.AddUIScrollItems(ref self.scrollServerItems,count);
			self.View.ELoopScrollList_SeverListLoopVerticalScrollRect.SetVisible(true,count);
		}
		
		public static void HideWindow(this DlgServer self)
		{
			self.RemoveUIScrollItems(ref self.scrollServerItems);
		}

		public static void OnScrollItemRefreshHandler(this DlgServer self, Transform transform, int index)
		{
			Scroll_Item_ServerInfo serverTest = self.scrollServerItems[index];
			serverTest.BindTrans(transform);
			ServerInfo info = self.Root().GetComponent<ClientServerInfosComponent>().ServerInfoList[index];
			serverTest.E_bgImage.color = info.Id == self.Root().GetComponent<ClientServerInfosComponent>().CurrentServerId? Color.red : Color.yellow;
			serverTest.ELabel_NumText.SetText(info.Id.ToString());
			serverTest.ELabel_NameText.SetText(info.ServerName);
			serverTest.EButton_SelectButton.AddListener(() => {  self.OnSelectServerItemHandler(info.Id);  });
		
		}

		public static void OnSelectServerItemHandler(this DlgServer self, long serverId)
		{
			self.Root().GetComponent<ClientServerInfosComponent>().CurrentServerId = int.Parse(serverId.ToString()) ;
			Log.Debug($"当前选择的服务器 Id 是:{serverId}");
			self.View.ELoopScrollList_SeverListLoopVerticalScrollRect.RefillCells();
		}
		
		public static async ETTask OnConfirmClickHandler(this DlgServer self)
		{
			bool isSelect = self.Root().GetComponent<ClientServerInfosComponent>().CurrentServerId != 0;

			if (!isSelect)
			{
				Log.Error("请先选择区服");
				return;
			}

			try
			{
				// int errorCode = await LoginHelper.GetRoles(self.ClientScene());
				// if (errorCode != ErrorCode.ERR_Success)
				// {
				// 	Log.Error(errorCode.ToString());
				// 	return;
				// }
				//
				// self.ClientScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Roles);
				// self.ClientScene().GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Server);
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

	}
}
