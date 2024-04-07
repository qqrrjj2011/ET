
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgLogin))]
	[EnableMethod]
	public  class DlgLoginViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Text E_AccountTextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AccountTextText == null )
     			{
		    		this.m_E_AccountTextText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Panel/Account/E_AccountText");
     			}
     			return this.m_E_AccountTextText;
     		}
     	}

		public UnityEngine.UI.Text E_PassWordTextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_PassWordTextText == null )
     			{
		    		this.m_E_PassWordTextText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Panel/Password/E_PassWordText");
     			}
     			return this.m_E_PassWordTextText;
     		}
     	}

		public UnityEngine.UI.Button E_LoginBtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginBtnButton == null )
     			{
		    		this.m_E_LoginBtnButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Panel/E_LoginBtn");
     			}
     			return this.m_E_LoginBtnButton;
     		}
     	}

		public UnityEngine.UI.Image E_LoginBtnImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginBtnImage == null )
     			{
		    		this.m_E_LoginBtnImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Panel/E_LoginBtn");
     			}
     			return this.m_E_LoginBtnImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_AccountTextText = null;
			this.m_E_PassWordTextText = null;
			this.m_E_LoginBtnButton = null;
			this.m_E_LoginBtnImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_AccountTextText = null;
		private UnityEngine.UI.Text m_E_PassWordTextText = null;
		private UnityEngine.UI.Button m_E_LoginBtnButton = null;
		private UnityEngine.UI.Image m_E_LoginBtnImage = null;
		public Transform uiTransform = null;
	}
}
