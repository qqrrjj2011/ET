namespace ET.Client
{
    [FriendOf(typeof(ET.ChatInfo))]
    [MessageHandler(SceneType.Demo)]
    public class Chat2C_NoticeChatInfoHandler : MessageHandler<Scene,Chat2C_NoticeChatInfo>
    {
        protected override async ETTask Run(Scene root, Chat2C_NoticeChatInfo message)
        {
            ChatInfo chatInfo = root.GetComponent<ChatComponent>().AddChild<ChatInfo>(true);
            chatInfo.Name     = message.Name;
            chatInfo.Message  = message.ChatMessage;
            root.GetComponent<ChatComponent>().Add(chatInfo);
            EventSystem.Instance.Publish(root, new EventType.UpdateChatInfo(){ZoneScene = root});
            await ETTask.CompletedTask;
        }
    }
}
