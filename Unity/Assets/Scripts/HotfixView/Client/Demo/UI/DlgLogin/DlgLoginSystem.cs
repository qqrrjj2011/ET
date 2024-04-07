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
				string account = self.View.E_AccountTextText.text.Trim();
				string password = self.View.E_PassWordTextText.text.Trim();
				
				Log.Info(">>>>>>>login click");
				LoginHelper.Login(self.Root(), account, password).Coroutine();
			});
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
		}
	}
}
