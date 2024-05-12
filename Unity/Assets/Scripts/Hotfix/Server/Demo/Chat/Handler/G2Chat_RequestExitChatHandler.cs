using System;

namespace ET.Server
{
    [MessageHandler(SceneType.ChatInfo)]
    public class G2Chat_RequestExitChatHandler : MessageHandler<ChatInfoUnit, G2Chat_RequestExitChat,Chat2G_RequestExitChat>
    {
        protected override async ETTask Run(ChatInfoUnit unit, G2Chat_RequestExitChat request, Chat2G_RequestExitChat response)
        {

            ChatInfoUnitsComponent chatInfoUnitsComponent = unit.Root().GetComponent<ChatInfoUnitsComponent>();
            
            chatInfoUnitsComponent.Remove(unit.Id);

          //  reply();
            
            await ETTask.CompletedTask;
        }
    }
}