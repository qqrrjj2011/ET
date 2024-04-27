using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgRole))]
	[FriendOf(typeof(RoleInfosComponent))]
	[FriendOf(typeof(RoleInfo))]
	public static  class DlgRoleSystem
	{
		 
		public static void RegisterUIEvent(this DlgRole self)
		{
			// 创建角色
			self.View.EButton_CreateButton.AddListenerAsync(async () =>
			{
				string roleName = self.View.E_InputFieldInputField.text.Trim();
				if (string.IsNullOrEmpty(roleName))
				{
					Log.Error("Name is null");
					return;
				}
				
				
				try
				{
					int errorCode = await LoginHelper.CreateRole(self.Root(),roleName);
					if (errorCode != ErrorCode.ERR_Success)
					{
						Log.Error(errorCode.ToString());
						return;
					}
					self.RefreshRoleItems();
				}
				catch (Exception e)
				{
					Log.Error(e.ToString());
				}
			});
			
			
			// 删除
			self.View.EButton_DelButton.AddListenerAsync(async () =>
			{
				if (self.Root().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
				{
					Log.Error("请选择需要删除的角色");
					return;
				}
				
				try
				{
					int error_code = await LoginHelper.DeleteRole(self.Root());
					if (error_code != ErrorCode.ERR_Success)
					{
						Log.Error(error_code.ToString());
						return;
					}

					self.RefreshRoleItems();
				}
				catch (Exception e)
				{
					Log.Error(e.ToString());
				}
				
			});
			
			// 滚动事件
			self.View.ELoopScrollList_RoleLoopVerticalScrollRect.AddItemRefreshListener((Transform tr, int index) =>
			{
				Scroll_Item_role itemRole = self.roleLst[index];
				itemRole.BindTrans(tr);

				RoleInfo roleInfo = self.Root().GetComponent<RoleInfosComponent>().RoleInfos[index];
				itemRole.ELabel_NumText.text = roleInfo.Id + "";
				itemRole.ELabel_RoleNameText.text = roleInfo.Name;
				itemRole.EButton_SelectButton.onClick.AddListener(() =>
				{
					self.Root().GetComponent<RoleInfosComponent>().CurrentRoleId = roleInfo.Id;
					self.View.ELoopScrollList_RoleLoopVerticalScrollRect.RefillCells();
				});
				itemRole.E_BgImage.color = roleInfo.Id == self.Root().GetComponent<RoleInfosComponent>().CurrentRoleId? Color.red : Color.yellow;

			});
			
			
			// 进入游戏
			self.View.EButton_EnterButton.AddListenerAsync(async () =>
			{
				if (self.Root().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
				{
					Log.Error("请选择需要删除的角色");
					return;
				}

				try
				{
					int error_code = await LoginHelper.GetRealmKey(self.Root());
					if (error_code != ErrorCode.ERR_Success)
					{
						Log.Error(error_code.ToString());
						return;
					}

					error_code = await LoginHelper.EnterGame(self.Root());
					if (error_code != ErrorCode.ERR_Success)
					{
						Log.Error(error_code.ToString());
						return;
					}
					self.Root().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Role);
				}
				catch (Exception e)
				{
					Log.Error(e.ToString());
				}
			});
		}

		public static void ShowWindow(this DlgRole self, Entity contextData = null)
		{
			self.RefreshRoleItems();
		}

		public static void HideWindow(this DlgRole self)
		{
			self.RemoveUIScrollItems(ref self.roleLst);
		}
		
		public static void RefreshRoleItems(this DlgRole self)
		{
			int count = self.Root().GetComponent<RoleInfosComponent>().RoleInfos.Count;
			self.AddUIScrollItems(ref self.roleLst,count);
			self.View.ELoopScrollList_RoleLoopVerticalScrollRect.SetVisible(true,count);
		}

	}
}
