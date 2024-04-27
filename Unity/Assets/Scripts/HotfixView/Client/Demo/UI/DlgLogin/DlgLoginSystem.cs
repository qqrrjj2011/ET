using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgLogin))]
	public static  class DlgLoginSystem
	{
		public static void RegisterUIEvent(this DlgLogin self)
		{
			self.View.E_LoginBtnButton.AddListener(() =>
			{
				self.LoginBtnEvent().Coroutine();
			});
		}

		static async ETTask LoginBtnEvent(this DlgLogin self)
		{
			string account = self.View.E_AccountTextText.text.Trim();
			string password = self.View.E_PassWordTextText.text.Trim();
				
			Log.Info(">>>>>>>login click");
			await LoginHelper.Login(self.Root(), account, password);
			
			self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Server);
			self.Root().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
		}
	}
}
