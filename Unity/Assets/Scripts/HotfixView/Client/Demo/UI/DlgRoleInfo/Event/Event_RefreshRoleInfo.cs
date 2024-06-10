
namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class Event_RefreshRoleInfo:AEvent<Scene,EventClientType.RefreshRoleInfo>
    {
        protected override async ETTask Run(Scene scene, EventClientType.RefreshRoleInfo a)
        {
            scene.GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>()?.Refresh();
            await ETTask.CompletedTask;
        }
    }
}

