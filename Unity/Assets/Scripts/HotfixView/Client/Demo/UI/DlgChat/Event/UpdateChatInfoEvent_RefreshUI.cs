using ET.Client;
using ET.EventType;

namespace ET.Client
{
    [FriendOf(typeof(UpdateChatInfo))]
    [Event(SceneType.Demo)]
    public class UpdateChatInfoEvent_RefreshUI : AEvent<Scene,UpdateChatInfo>
    {
        protected override async ETTask Run(Scene scene,UpdateChatInfo args)
        {
            scene.Root().GetComponent<UIComponent>()?.GetDlgLogic<DlgChat>()?.Refresh();
            await ETTask.CompletedTask;
        }
    }
}