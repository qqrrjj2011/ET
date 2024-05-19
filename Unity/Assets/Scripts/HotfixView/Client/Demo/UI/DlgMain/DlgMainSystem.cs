using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgMain))]
	public static  class DlgMainSystem
	{

		public static void RegisterUIEvent(this DlgMain self)
		{
			self.View.E_BagButtonButton.GetComponent<Button>().AddListenerAsync( async () =>
			{
				self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Bag);
				await ETTask.CompletedTask;
			});
			
			self.View.E_ChatButtonButton.GetComponent<Button>().AddListenerAsync( async () =>
			{
				self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Chat);
				await ETTask.CompletedTask;
			});
			
			self.View.E_RankButtonButton.GetComponent<Button>().AddListenerAsync( async () =>
			{
				self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Rank);
				await ETTask.CompletedTask;
			});
			
			self.View.E_TaskButtonButton.GetComponent<Button>().AddListenerAsync( async () =>
			{
				self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Task);
				await ETTask.CompletedTask;
			});
			
			self.View.E_ForgeButtonButton.GetComponent<Button>().AddListenerAsync( async () =>
			{
				self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Forge);
				await ETTask.CompletedTask;
			});
		}

		public static void ShowWindow(this DlgMain self, Entity contextData = null)
		{
			
		}

		 

	}
}
