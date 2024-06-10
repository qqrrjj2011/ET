using System;

namespace ET.Server
{
    [FriendOf(typeof(ChatInfoUnitsComponent))]
    [FriendOf(typeof(ChatInfoUnit))]
    [MessageHandler(SceneType.ChatInfo)]
    public class C2Chat_SendChatInfoHandler : MessageHandler<ChatInfoUnit, C2Chat_SendChatInfo, Chat2C_SendChatInfo>
    {
        protected override async ETTask Run(ChatInfoUnit chatInfoUnit, C2Chat_SendChatInfo request, Chat2C_SendChatInfo response)
        {
            Log.Warning(">>>>>>>>>>C2Chat_SendChatInfoHandler run");
            if (string.IsNullOrEmpty(request.ChatMessage))
            {
                response.Error = ErrorCode.ERR_ChatMessageEmpty;
               // reply();
                return;
            }
            ChatInfoUnitsComponent chatInfoUnitsComponent = chatInfoUnit.Root().GetComponent<ChatInfoUnitsComponent>();
            foreach (var otherUnit in chatInfoUnitsComponent.ChatInfoUnitsDict.Values)
            {
                ChatInfoUnit ent = otherUnit;
                Chat2C_NoticeChatInfo chat2CNoticeChatInfo = Chat2C_NoticeChatInfo.Create();
                chat2CNoticeChatInfo.Name = chatInfoUnit.Name;
                chat2CNoticeChatInfo.ChatMessage = request.ChatMessage;
             //   chatInfoUnit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(ent.Id, chat2CNoticeChatInfo);
               chatInfoUnit.Root().GetComponent<MessageSender>().Send(ent.PlayerSessionComponentActorId, chat2CNoticeChatInfo);
            }

           // reply();
            await ETTask.CompletedTask;
        }
    }
}