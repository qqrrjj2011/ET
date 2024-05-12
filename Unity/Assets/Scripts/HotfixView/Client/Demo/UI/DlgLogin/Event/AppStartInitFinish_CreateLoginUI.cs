namespace ET.Client
{
	[Event(SceneType.Demo)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, EventType.AppStartInitFinish>
	{
		protected override async ETTask Run(Scene root, EventType.AppStartInitFinish args)
		{
			await root.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Login);
		}
	}
}
